// Learn more about F# at http://fsharp.org

open System
open System.IO
open System.Collections.Generic
open System.Threading
open System.Collections.Concurrent
open FSharp.Collections.ParallelSeq
open System.Text.RegularExpressions
open FSharp.Configuration

type TreeNode = {    
    Name : string  
    mutable Parent : TreeNode   
    Children : TreeNode ConcurrentBag
} with   
   member x.is_root() = x.Name = "DNS root"
   member x.level() =
          if x.is_root() then 0 else x.Parent.level() + 1
   member x.is_leaf() = x.Children.Count = 0

let count_chr x xs =  xs
                      |> Seq.filter (fun x' -> x' = x)
                      |> Seq.length

let find tree name level = 
    let rec findInner tnode =
        match tnode with
        | { Name = n; Parent = _; Children = _ } as tn when tn.level() = level && n = name -> Some(tn)
        | { Name = _; Parent = _; Children = children } -> children |> Seq.choose (findInner) 
                                                                    |> Seq.tryFind (fun _ -> true)
        | { Name = _; Parent = _; Children = c } when c.Count = 0  -> None
    findInner tree

let findTreeNodeByName(name:string, root:TreeNode, level:int) =
    find root name level

let dict_level_treenode = new ConcurrentDictionary<int, ConcurrentBag<TreeNode>>()

let ffindTreeNodeByName(name:string, root:TreeNode, level:int) =
    if dict_level_treenode.ContainsKey(level) then
       dict_level_treenode[level] |> Seq.tryFind(fun n -> n.Name = name)
    else
       None

let lockobj = new obj();

let lock (lockobj:obj) f =
  Monitor.Enter lockobj
  try
    f()
  finally
    Monitor.Exit lockobj

let createTreeNode(dns_name:string, root:TreeNode) =
    let point_char = '.'
    let mutable level = 0
    let mutable parent = root
    if count_chr point_char dns_name > 0 then
       let splitted = dns_name.Split [| point_char |]
       for i = splitted.Length - 1 downto 0 do
         level <- level + 1
         let carr_data = splitted[i..splitted.Length - 1]
         let cdata = ('.', carr_data) |> System.String.Join

         match (ffindTreeNodeByName(cdata,root,level)) with
         | Some(n) -> parent <- n
         | None -> let cnode = { Name = cdata; Parent = parent; Children = ConcurrentBag<TreeNode>() }
                   parent.Children.Add(cnode)
                   parent <- cnode

                   let level = cnode.level()
                   
                   if not(dict_level_treenode.ContainsKey(level)) then
                      let arr = new ConcurrentBag<TreeNode>() 
                      let current = dict_level_treenode.GetOrAdd(level, arr)
                      lock lockobj (fun _ -> current.Add(cnode))
                   else
                      lock lockobj (fun _ -> dict_level_treenode[level].Add(cnode))

let flfindTreeNodeByName(name:string, level:int, dict_level_treenode_local:ConcurrentDictionary<int, ConcurrentBag<TreeNode>>) =
    if dict_level_treenode_local.ContainsKey(level) then
       dict_level_treenode_local[level] |> PSeq.tryFind(fun n -> n.Name = name)
    else
       None

let fastFindTreeNodeByName(name:string, dict_treenode: ConcurrentDictionary<string, TreeNode>) =
    if dict_treenode.ContainsKey(name) then
       Some(dict_treenode[name])
    else
       None

let createTreeNodeLocal(dns_name:string, root:TreeNode, dict_treenode: ConcurrentDictionary<string, TreeNode>, dict_level_treenode_local:ConcurrentDictionary<int, ConcurrentBag<TreeNode>>) =
    let point_char = '.'
    let mutable level = 0
    let mutable parent = root
    if count_chr point_char dns_name > 0 then
       let splitted = dns_name.Split [| point_char |]
       for i = splitted.Length - 1 downto 0 do
         level <- level + 1
         let carr_data = splitted[i..splitted.Length - 1]
         let cdata = ('.', carr_data) |> System.String.Join

         match (flfindTreeNodeByName(cdata, level, dict_level_treenode_local)) with
         | Some(n) -> parent <- n
         | None -> let cnode = { Name = cdata; Parent = parent; Children = ConcurrentBag<TreeNode>() }
                   parent.Children.Add(cnode)
                   parent <- cnode

                   let level = cnode.level()
                   
                   if not(dict_level_treenode_local.ContainsKey(level)) then
                      let arr = new ConcurrentBag<TreeNode>() 
                      let current = dict_level_treenode_local.GetOrAdd(level, arr)
                      lock lockobj (fun _ -> current.Add(cnode))
                   else
                      lock lockobj (fun _ -> dict_level_treenode_local[level].Add(cnode))

let fastCreateTreeNodeLocal(dns_name:string, root:TreeNode, dict_treenode: ConcurrentDictionary<string, TreeNode>, dict_level_treenode_local:ConcurrentDictionary<int, ConcurrentBag<TreeNode>>) =
    let point_char = '.'
    let mutable level = 0
    let mutable parent = root
    if count_chr point_char dns_name > 0 then
       let splitted = dns_name.Split [| point_char |]
       for i = splitted.Length - 1 downto 0 do
         level <- level + 1
         let carr_data = splitted[i..splitted.Length - 1]
         let cdata = ('.', carr_data) |> System.String.Join

         match (fastFindTreeNodeByName(cdata, dict_treenode)) with
         | Some(n) -> parent <- n
         | None -> let cnode = { Name = cdata; Parent = parent; Children = ConcurrentBag<TreeNode>() }
                   parent.Children.Add(cnode)
                   parent <- cnode

                   let level = cnode.level()
                   
                   if not(dict_level_treenode_local.ContainsKey(level)) then
                      let arr = new ConcurrentBag<TreeNode>() 
                      let current = dict_level_treenode_local.GetOrAdd(level, arr)
                      lock lockobj (fun _ -> current.Add(cnode))
                   else
                      lock lockobj (fun _ -> dict_level_treenode_local[level].Add(cnode))

                   if not(dict_treenode.ContainsKey(cdata)) then
                      dict_treenode.GetOrAdd(cdata, fun _ -> cnode) |> ignore
  
let dispose_sw(sw:StreamWriter) =
    sw.Flush()
    sw.Close()

let writeResult(level:int, wildcardoutput:string, wildcardoutputdet:string, singleoutput:string) =

    use sww = File.AppendText(wildcardoutput)
    use swd = File.AppendText(wildcardoutputdet)
    use swc = File.AppendText(singleoutput)

    if dict_level_treenode.ContainsKey(level) then
       dict_level_treenode[level] |> Seq.iter(fun n -> let count = n.Children.Count
                                                       if count > 1 then 
                                                          let wcard = "*." + n.Name
                                                          sww.WriteLine(wcard)

                                                          swd.WriteLine(wcard)
                                                          swd.WriteLine("# Count - " + (n.Children.Count |> string))
                                                          n.Children |> Seq.iter(fun tcn -> swd.WriteLine("#" + tcn.Name))
                                                       else
                                                          if count = 0 then 
                                                             swc.WriteLine(n.Name) 
                                                          else 
                                                             let finded = dict_level_treenode[level + 1] |> Seq.find(fun cni -> obj.ReferenceEquals(cni.Parent, n))
                                                             swc.WriteLine(finded.Name) 
                                              )
    dispose_sw(sww)
    dispose_sw(swd)
    dispose_sw(swc)

let writeResultTreeLocal(wildcardoutput:string, singleoutput:string, dict_level_treenode_local:ConcurrentDictionary<int, ConcurrentBag<TreeNode>>) =

    use sww = File.AppendText(wildcardoutput)
    use sws = File.AppendText(singleoutput)

    dict_level_treenode_local.Keys 
    |> Seq.sort 
    |> Seq.skip 1 // level 1 .com .net .dev etc
    |> Seq.rev 
    |> Seq.iter(fun level -> dict_level_treenode_local[level]
                             |> Seq.iter(fun n -> let ncount = n.Parent.Children.Count
                                                  match ncount with 
                                                  | nc when nc = 1 -> sws.WriteLine(n.Name) // single node
                                                  | ncp when ncp > 1 -> sww.WriteLine("*." + n.Name) // wildcard node                    
                                                  | _ -> () 
                                        ))
    dispose_sw(sww)
    dispose_sw(sws)

let collectSingleTreeNodes(singlenodeoutput:string, dict_level_treenode_local:ConcurrentDictionary<int, ConcurrentBag<TreeNode>>) =

    let resize = new ResizeArray<string>()

    use sws = File.AppendText(singlenodeoutput)

    dict_level_treenode_local.Keys 
    |> Seq.sort 
    |> Seq.skip 1 // level 1 .com .net .dev etc
    |> Seq.rev
    
    |> Seq.iter(fun level -> sws.WriteLine("# Collected on Tree level - " + (level |> string))
                             dict_level_treenode_local[level]
                             |> Seq.filter(fun node -> node.Parent.Children.Count = 1 && node.Children.Count = 0)
                             |> Seq.iter(fun n -> let ncount = n.Parent.Children.Count
                                                  match ncount with 
                                                  | nc when nc = 1 -> let mutable current = n
                                                                      let mutable breakl = false

                                                                      while not(current.is_root()) && not(breakl) do
                                                                          if current.Children.Count > 1 then
                                                                             breakl <- true
                                                                          current <- current.Parent

                                                                      if not(breakl) then
                                                                         resize.Add(n.Name) // single node
                                                                         sws.WriteLine(n.Name) // single node
                                                  | _ -> () 
                                        ))
    dispose_sw(sws)
    resize.Clear()

let collectWildcardTreeNodes(wildcardnodeoutput:string, dict_level_treenode_local:ConcurrentDictionary<int, ConcurrentBag<TreeNode>>) =

    let resize = new ResizeArray<string>()

    use sws = File.AppendText(wildcardnodeoutput)

    dict_level_treenode_local.Keys 
    |> Seq.sort 
    |> Seq.skip 1 // level 1 .com .net .dev etc
    |> Seq.rev
    
    |> Seq.iter(fun level -> sws.WriteLine("\n")
                             sws.WriteLine("# Collected on Tree level - " + (level |> string) + " Count - " + (dict_level_treenode_local[level].Count |> string))
                             sws.WriteLine("\n")
                             dict_level_treenode_local[level]

                             |> Seq.filter(fun node -> node.Children.Count > 1)
                             |> Seq.iter(fun n -> let wcard = "*." + n.Name
                                                  resize.Add(wcard) // wildcard node
                                                  sws.WriteLine(wcard) // wildcard node
                                        ))
    dispose_sw(sws)
    resize.Clear()

let collectWildcardTreeNodesByLevel(wildcardnodeoutput:string, dict_level_treenode_local:ConcurrentDictionary<int, ConcurrentBag<TreeNode>>) =

    let resize = new ResizeArray<string>()

    dict_level_treenode_local.Keys 
    |> Seq.sort 
    |> Seq.skip 1 // level 1 .com .net .dev etc
    |> Seq.rev
    
    |> Seq.iter(fun level -> use swс = File.AppendText(Path.Combine(wildcardnodeoutput, (sprintf "collectLevel-%d.txt" level)))

                             dict_level_treenode_local[level]
                             |> Seq.filter(fun node -> node.Children.Count > 1)
                             |> Seq.iter(fun n -> let wcard = "*." + n.Name
                                                  resize.Add(wcard) // wildcard node
                                                  swс.WriteLine(wcard) ) // wildcard node

                             dispose_sw(swс)           
                             )
    resize.Clear()

let writeResultLocal(level:int, wildcardoutput:string, wildcardoutputdet:string, dict_level_treenode_local:ConcurrentDictionary<int, ConcurrentBag<TreeNode>>) =
    let wdict = Dictionary<string, int>()

    let wdict_add(str:string) =
        if not(wdict.ContainsKey(str)) then
            wdict.Add(str, 1)

    if File.Exists wildcardoutput then
       let lines = File.ReadLines(wildcardoutput)
       lines |> Seq.iter(fun line -> wdict_add(line))

    //let sdict = Dictionary<string, int>()

    //let sdict_add(str:string) =
    //    if not(sdict.ContainsKey(str)) then
    //        sdict.Add(str, 1)

    //if File.Exists singleoutput then
    //    let lines = File.ReadLines(singleoutput)
    //    lines |> Seq.iter(fun line -> sdict_add(line))

    use sww = File.AppendText(wildcardoutput)
    use swd = File.AppendText(wildcardoutputdet)
    //use swc = File.AppendText(singleoutput)

    if dict_level_treenode_local.ContainsKey(level) then
       dict_level_treenode_local[level] |> Seq.iter(fun n -> let count = n.Children.Count
                                                             match count with 
                                                             //| c when c = 0 -> sdict_add(n.Name)
                                                             | cnt when cnt > 1 -> let wcard = "*." + n.Name
                                                                                   wdict_add(wcard)
                                                                                   swd.WriteLine(wcard)
                                                                                   swd.WriteLine("# Count - " + (n.Children.Count |> string))
                                                                                   n.Children |> Seq.iter(fun tcn -> swd.WriteLine("#" + tcn.Name))
                                                             | _ -> ()
                                              )
    dispose_sw(swd)

    wdict.Keys |> Seq.iter(fun wcard -> sww.WriteLine(wcard))
    dispose_sw(sww)
    wdict.Clear()
   
    //sdict.Keys |> Seq.iter(fun single -> swc.WriteLine(single))
    //dispose_sw(swc)
    //sdict.Clear()

let writeResultWildcardLocal(level:int, wildcardoutput:string, wildcardoutputdet:string, singleoutput:string, dict_level_treenode_local:ConcurrentDictionary<int, ConcurrentBag<TreeNode>>) =
    let wdict = Dictionary<string, int>()

    let wdict_add(str:string) =
        if not(wdict.ContainsKey(str)) then
            wdict.Add(str, 1)

    if File.Exists wildcardoutput then
       let lines = File.ReadLines(wildcardoutput)
       lines |> Seq.iter(fun line -> wdict_add(line))

    let sdict = Dictionary<string, int>()
    
    let sdict_add(str:string) =
        if not(sdict.ContainsKey(str)) then
           sdict.Add(str, 1)
    
    if File.Exists singleoutput then
       let lines = File.ReadLines(singleoutput)
       lines |> Seq.iter(fun line -> sdict_add(line))

    use sww = File.AppendText(wildcardoutput)
    use swd = File.AppendText(wildcardoutputdet)
    use swc = File.AppendText(singleoutput)

    if dict_level_treenode_local.ContainsKey(level) then
       dict_level_treenode_local[level] |> Seq.iter(fun n -> let count = n.Children.Count
                                                             match count with
                                                             | c when c = 0 -> sdict_add(n.Name)
                                                             | c when c = 1 -> let finded = dict_level_treenode_local[level + 1] |> Seq.find(fun cni -> obj.ReferenceEquals(cni.Parent, n))
                                                                               sdict_add(finded.Name)
                                                             | cnt when cnt > 1 -> let wcard = "*." + n.Name
                                                                                   wdict_add(wcard)

                                                                                   swd.WriteLine(wcard)
                                                                                   swd.WriteLine("# Count - " + (n.Children.Count |> string))
                                                                                   n.Children |> Seq.iter(fun tcn -> swd.WriteLine("#" + tcn.Name))
                                                             | _ -> ()
                                              )
    dispose_sw(swd)

    wdict.Keys |> Seq.iter(fun wcard -> sww.WriteLine(wcard))
    dispose_sw(sww)

    let lines = File.ReadAllLines(wildcardoutput)
    lines |> Seq.iter(fun line -> wdict_add(line)) 

    sdict.Keys |> Seq.iter(fun single -> let splitted = single.Split(".")
                                         let key = "*." + splitted[splitted.Length-2] + "." + splitted[splitted.Length-1]
                                         if not(wdict.ContainsKey(key)) then
                                            swc.WriteLine(single))
    dispose_sw(swc)

    sdict.Clear()
    wdict.Clear()

let revere_str (s:string) = s |> Array.ofSeq |> Array.rev |> System.String

let writeData(output:string, data: IEnumerable<string>) =

    use sw = File.AppendText(output)
    data |> Seq.iter(fun s -> sw.WriteLine(s))
    dispose_sw(sw) 
    
let replace_digits_in_domaintld(input:string, output:string, outputdist:string) =
    
    let resize = new ResizeArray<string>()
    let dict_filter = new Dictionary<string, int>()

    let dict_filter_add(str:string) =
        if not(dict_filter.ContainsKey(str)) then
           dict_filter.Add(str, 1)

    [| output;
       outputdist;
    |] 
    |> Seq.iter(fun f -> if File.Exists(f) then File.Delete(f))

    let lines = File.ReadAllLines(input)
    lines |> Seq.iter(fun line -> let splitted = line.Split(".")
                                  let data = splitted |> Seq.map(fun x -> 
                                                                          let output = Regex.Replace(x, @"\d{2,}", "@")
                                                                          let result = Regex.Replace(output, @"\d", "[0-9]")
                                                                          let result = Regex.Replace(result, "@", "[0-9]*")
                                                                          result
                                                                          )
                                                               
                                  let newval = ('.', data) |> System.String.Join
                                  resize.Add(newval) 
                                  dict_filter_add(newval)
                    )  

    writeData(output, resize)
    writeData(outputdist, dict_filter.Keys)

    resize.Clear()
    dict_filter.Clear()

let reverse_sort_str(lines:IEnumerable<string>) =
    lines |> Seq.map revere_str |> Seq.sort |> Seq.map revere_str

let is_valid_domain_name(domain:string) =
    match domain with
    | d when String.IsNullOrEmpty(d) || d.Length > 253 -> false
    | _ as dn -> let splitted = domain.Split('.')
                 let res = splitted |> Seq.filter(fun s->s.Length > 63)
                 (Seq.length res = 0) &&  not(Uri.CheckHostName(dn) = UriHostNameType.Unknown)

let filter_dns_bytld(input:string, output:string, normal_domain_tld:ResizeArray<string>) =

    [| 
       output;
    |] 
    |> Seq.iter(fun f -> if File.Exists(f) then File.Delete(f))

    let dict = new Dictionary<string, int>()
    
    let dict_add(str:string) =
        if not(dict.ContainsKey(str)) then
            dict.Add(str, 1)
    
    let lines = File.ReadAllLines(input)

    normal_domain_tld |> Seq.iter(fun tld -> let tldn = tld.TrimStart([|'*'|])
                                             let finded = lines |> Seq.tryFind(fun line -> line.EndsWith(tldn))
                                             match finded with
                                             | Some(t) -> dict_add(t)
                                             | None -> ())
    
    writeData(output, reverse_sort_str(dict |> Seq.map (fun item -> item.Key)))

    dict.Clear()

// split input source dnsnames file into two files: 
// valid dnsnames and not valid dnsnames
let splitSourceFileToValidNotValidDNSName(inputsource:string, outputvalid:string, outputnotvalid:string) =
    let resize = new ResizeArray<string>()

    [| 
       outputvalid;
       outputnotvalid;
    |] 
    |> Seq.iter(fun f -> if File.Exists(f) then File.Delete(f))

    let resize_notvalid = new ResizeArray<string>()

    let lines = File.ReadAllLines(inputsource)
    lines |> Seq.iter(fun line -> let cline = line.Replace("0.0.0.0 ", String.Empty).Replace(":443", String.Empty)
                                  if is_valid_domain_name(cline) then resize.Add(cline) else resize_notvalid.Add(cline)) 


    File.WriteAllLines(outputvalid, resize)
    File.WriteAllLines(outputnotvalid, resize_notvalid)

    resize.Clear()
    resize_notvalid.Clear()

// split input source dnsnames file into two files: 
// multipoint dnsnames and one point dnsnames
let splitSourceFileToMultiAndOnePointDNSName(inputsource:string, outputmultipoint:string, outputonepoint:string) =
    let resize = new ResizeArray<string>()
    let dict_filter = new Dictionary<string, int>()

    [| 
        outputmultipoint;
        outputonepoint;
    |] 
    |> Seq.iter(fun f -> if File.Exists(f) then File.Delete(f))
   

    let filter_dns_one_point (str:string) = 
        match str with
        | t when count_chr '.' t = 1 -> resize.Add(t)
        | z when count_chr '.' z = 0 -> ()
        | _ as s -> if not(dict_filter.ContainsKey(s)) then 
                       dict_filter.Add(s, 1)

    let lines = File.ReadAllLines(inputsource)
    lines |> Seq.iter(fun line -> filter_dns_one_point(line) |> ignore) 

    File.WriteAllLines(outputonepoint, resize)
    File.WriteAllLines(outputmultipoint, dict_filter.Keys) 

    resize.Clear()
    dict_filter.Clear()

let collectDomainTldStatisticsConcurrent(inputsource:string, wcardoutput:string, singlewcardoutput:string) =
    let cdict = new ConcurrentDictionary<string, int * string>()

    [| 
    wcardoutput;
    singlewcardoutput;
    |] 
    |> Seq.iter(fun f -> if File.Exists(f) then File.Delete(f)) 

    let cdict_add(str:string,original:string) =
     if not(cdict.ContainsKey(str)) then
         cdict.GetOrAdd(str, (1,original)) |> ignore
     else
         let (current,orig) = cdict.GetOrAdd(str, fun _ -> (100000000,original))
         let count = ref current
         let cval = Interlocked.Increment(count)
         cdict.AddOrUpdate(str, Func<string, (int * string)>(fun _ -> (cval,orig)), Func<string, (int * string), (int * string)>(fun _ _ -> (cval,orig))) |> ignore

    let lines = File.ReadAllLines(inputsource)

    lines |> Seq.iter(fun line -> let splitted = line.Split(".")
                                  let key = "*." + splitted[splitted.Length-2] + "." + splitted[splitted.Length-1]
                                  cdict_add(key, line) )  
                               
    writeData(wcardoutput, (cdict |> Seq.filter(fun item -> let (count,_) = item.Value 
                                                            count > 1) 
                                  |> Seq.map (fun item -> item.Key)) ) 

    writeData(singlewcardoutput, (cdict |> Seq.filter(fun item -> let (count,_) = item.Value 
                                                                  count = 1) 
                                        |> Seq.map (fun item -> let (_,orig) = item.Value  
                                                                orig)) )

    printfn "collectDomainTldStatisticsConcurrent synchronouse count %d" cdict.Keys.Count 

    cdict.Clear()  

    lines |> Seq.chunkBySize 200000 
    |> PSeq.iter (fun batchp -> batchp |> Array.Parallel.iter (fun line -> let splitted = line.Split(".")
                                                                           let key = "*." + splitted[splitted.Length-2] + "." + splitted[splitted.Length-1]
                                                                           cdict_add(key, line) ))

    printfn "collectDomainTldStatisticsConcurrent parallel count %d" cdict.Keys.Count
    cdict.Clear()

    
let collectDomainTldStatistics(inputsource:string, wcardoutput:string) =

    let dict = new Dictionary<string, int>()

    [| 
      wcardoutput;
    |] 
    |> Seq.iter(fun f -> if File.Exists(f) then File.Delete(f)) 
    
    let dict_add(str:string) =
        if not(dict.ContainsKey(str)) then
            dict.Add(str, 1)
        else
            let current = dict[str]
            dict[str] <- (current + 1)
            
    let lines = File.ReadAllLines(inputsource)

    lines |> Seq.iter(fun line -> let splitted = line.Split(".")
                                  let key = "*." + splitted[splitted.Length-2] + "." + splitted[splitted.Length-1]
                                  dict_add(key) )  
                                  
    writeData(wcardoutput, (dict |> Seq.filter(fun item -> item.Value > 1) |> Seq.map (fun item -> item.Key)))

    dict.Clear()

let processingOnePointDomainTldStatistics(inputsource:string, singleoutput:string) =
    
        let cdict = new ConcurrentDictionary<string, int * string>()
    
        [| 
          singleoutput;
        |] 
        |> Seq.iter(fun f -> if File.Exists(f) then File.Delete(f)) 
        
        let cdict_add(str:string,original:string) =
            if not(cdict.ContainsKey(str)) then
                cdict.GetOrAdd(str, (1,original)) |> ignore
            else
                let (current,orig) = cdict.GetOrAdd(str, fun _ -> (100000000,original))
                let count = ref current
                let cval = Interlocked.Increment(count)
                cdict.AddOrUpdate(str, Func<string, (int * string)>(fun _ -> (cval,orig)), Func<string, (int * string), (int * string)>(fun _ _ -> (cval,orig))) |> ignore
                
        let lines = File.ReadAllLines(inputsource)
    
        lines 
        |> Seq.iter(fun line -> let splitted = line.Split(".")
                                let key = "*." + splitted[splitted.Length-2] + "." + splitted[splitted.Length-1]
                                cdict_add(key, line) )    
                                      
        writeData(singleoutput, reverse_sort_str(cdict |> Seq.map (fun item -> item.Key) ) )
    
        cdict.Clear()

let processingOnePointDomainTldStatisticsWithCount(inputsource:string, singleoutput:string) = 
    
        let dict = new Dictionary<string, int>()

        let dict_add(str:string) =
            if not(dict.ContainsKey(str)) then
                dict.Add(str, 1)
            else
                let current = dict[str]
                dict[str] <- (current + 1)
    
        [| 
          singleoutput;
        |] 
        |> Seq.iter(fun f -> if File.Exists(f) then File.Delete(f)) 
        
        let writeKeyValuePairData(output:string, data: IEnumerable<KeyValuePair<string,int>>) =
            
            use sw = File.AppendText(output)
            data |> Seq.sortByDescending(fun p -> p.Value) |> Seq.iter(fun p -> sw.WriteLine(sprintf "%s # %d" p.Key p.Value))
            dispose_sw(sw)
        
            dict.Clear()
        
        let lines = File.ReadAllLines(inputsource) 
            
        
        lines |> Seq.iter(fun line -> let splitted = line.Split(".")
                                      let key = "*." + splitted.[splitted.Length-1]
                                      dict_add(key) ) 
        
        writeKeyValuePairData(singleoutput, dict)
        
        dict.Clear()

let filterDnsNamesByNormalTld(inputsource:string, normaltldoutput:string) = 
    let dict_valid_domain_tld = new Dictionary<string, int>()

    let dict_valid_domain_tld_add(str:string) =
        if not(dict_valid_domain_tld.ContainsKey(str)) then
            dict_valid_domain_tld.Add(str, 1)
        else
            let current = dict_valid_domain_tld[str]
            dict_valid_domain_tld[str] <- (current + 1)

    [| 
        normaltldoutput;
    |] 
    |> Seq.iter(fun f -> if File.Exists(f) then File.Delete(f))

    // filter by normal tld
    let valid_domain_tld = new ResizeArray<string>([ ".com"; ".net"; ".org"; ".dev"; ".blog"; ".page" ]) // ".gov"; ".de";

    let lines = File.ReadAllLines(inputsource) 

    lines |> Seq.iter(fun line -> let splitted = line.Split(".")
                                  let tld = splitted.[splitted.Length-1]
                                  let finded = valid_domain_tld |> Seq.tryFind(fun vd -> vd = "." + tld)
                                  match finded with
                                  | Some(_) -> dict_valid_domain_tld_add(line)
                                  | None -> ()
                                  ) 

    writeData(normaltldoutput, reverse_sort_str(dict_valid_domain_tld |> Seq.map (fun item -> item.Key))) 

    dict_valid_domain_tld.Clear()

let filterDnsNamesByTld(inputsource:string, tldoutput:string) = 
    let dict = new Dictionary<string, int>()

    let dict_add(str:string) =
        if not(dict.ContainsKey(str)) then
            dict.Add(str, 1)
        else
            let current = dict[str]
            dict[str] <- (current + 1)

    [| 
        tldoutput;
    |] 
    |> Seq.iter(fun f -> if File.Exists(f) then File.Delete(f))

    let lines = File.ReadAllLines(inputsource) 

    lines |> Seq.iter(fun line -> let splitted = line.Split(".")
                                  let key = "*." + splitted.[splitted.Length-1]
                                  dict_add(key)
                                  ) 

    let ntld_data = new ResizeArray<string>(dict |> Seq.filter(fun item -> item.Value = 1)
                                                 |> Seq.map (fun item -> item.Key))

    filter_dns_bytld(inputsource, tldoutput, ntld_data)

    dict.Clear()


let rem_comment(str:string) : string =
    let tstr = str.Trim()

    let indx = tstr.IndexOf("#")
    if tstr.Contains("#") && indx > 0 then
       tstr.Substring(0, indx - 1)
    else
       tstr

let is_valid_ipv4 (str:string) =
    let (isValidIp,_) = System.Net.IPAddress.TryParse(str)
    isValidIp


let filter_data(input:string, output:string) = 
    let dict_filtered_data = new Dictionary<string, int>()

    let dict_data_add(str:string) = 
        if not(dict_filtered_data.ContainsKey(str)) then 
            dict_filtered_data.Add(str, 1)

    let trim_after(str:string, chr:char) =
        let index = str.LastIndexOf(chr)
        if index > 0 then
           let ext = str.Substring(index, str.Length - index) 
           ext
        else
           str

    let replace_data(str:string) = 
        let text = str.Replace("www.", String.Empty).Replace("0.0.0.0 ", String.Empty).Replace(":443", String.Empty)
        let text = rem_comment(text)
        let text = trim_after(text, ':')
        if not(text.Contains("/")) && not(is_valid_ipv4(text)) then
           dict_data_add(text)

    let filter_dnsdata (str:string) = 
        match str with
        | tx when tx.StartsWith("#", StringComparison.CurrentCultureIgnoreCase) -> ()
        | _ as txt ->  replace_data(txt)


    let lines = File.ReadLines(input)
    lines |> Seq.iter(fun x -> filter_dnsdata(x.Trim()))
    File.WriteAllLines(output, dict_filtered_data.Keys)

    dict_filtered_data.Clear()

type Settings = AppSettings<"app.config"> 

let checkDirStructure() =
    if not(Directory.Exists(Settings.InputDataSourceRootDir)) then 
       Directory.CreateDirectory(Settings.InputDataSourceRootDir) |> ignore
       printfn "app.config InputDataSourceRootDir - %s  created" Settings.InputDataSourceRootDir
    if not(Directory.Exists(Settings.OutputTmpDir)) then 
       Directory.CreateDirectory(Settings.OutputTmpDir) |> ignore
       printfn "app.config OutputTmpDir - %s  created" Settings.OutputTmpDir
    if not(Directory.Exists(Settings.OutputResultDir)) then 
       Directory.CreateDirectory(Settings.OutputResultDir) |> ignore
       printfn "app.config OutputResultDir - %s  created" Settings.OutputResultDir
    if not(File.Exists(Path.Combine(Settings.InputDataSourceRootDir, Settings.InputDataSourceFileName))) then 
       printfn "app.config InputDataSourceFileName - %s not found." (Path.Combine(Settings.InputDataSourceRootDir, Settings.InputDataSourceFileName)) 
       exit -1

[<EntryPoint>]
let main _ =
    try
       checkDirStructure()

       printfn "Start processing dnsnames file ..."

       let startWatch = System.Diagnostics.Stopwatch.StartNew()
       
       filter_data(Path.Combine(Settings.InputDataSourceRootDir, Settings.InputDataSourceFileName), 
                   Path.Combine(Settings.OutputTmpDir, Settings.OutputFilteredDataSourceFileName))

       let stopWatch = System.Diagnostics.Stopwatch.StartNew()

       splitSourceFileToValidNotValidDNSName(Path.Combine(Settings.OutputTmpDir, Settings.OutputFilteredDataSourceFileName),
                                             Path.Combine(Settings.OutputTmpDir, Settings.ValidDnsNamesFileName),
                                             Path.Combine(Settings.OutputTmpDir, Settings.NotValidDnsNamesFileName))

       stopWatch.Stop()
       System.Console.WriteLine("splitSourceFileToValidNotValidDNSName - Time elapsed: {0}", stopWatch.Elapsed)

       let stopWatch = System.Diagnostics.Stopwatch.StartNew()

       splitSourceFileToMultiAndOnePointDNSName(Path.Combine(Settings.OutputTmpDir, Settings.ValidDnsNamesFileName),
                                                Path.Combine(Settings.OutputTmpDir, Settings.MultiPointDnsNamesFileName), 
                                                Path.Combine(Settings.OutputTmpDir, Settings.OnePointDnsNamesFileName))

       stopWatch.Stop()
       System.Console.WriteLine("splitSourceFileToMultiAndOnePointDNSName - Time elapsed: {0}", stopWatch.Elapsed)

       let stopWatch = System.Diagnostics.Stopwatch.StartNew()

       collectDomainTldStatisticsConcurrent(Path.Combine(Settings.OutputTmpDir, Settings.MultiPointDnsNamesFileName), 
                                            Path.Combine(Settings.OutputTmpDir, Settings.WildcardTldStatisticsCdIctFileName), 
                                            Path.Combine(Settings.OutputTmpDir, Settings.SingleTldStatisticsCdIctFileName) )

       stopWatch.Stop()
       System.Console.WriteLine("collectDomainTldStatisticsConcurrent - Time elapsed: {0}", stopWatch.Elapsed)

       let stopWatch = System.Diagnostics.Stopwatch.StartNew()

       collectDomainTldStatistics(Path.Combine(Settings.OutputTmpDir, Settings.MultiPointDnsNamesFileName),
                                  Path.Combine(Settings.OutputTmpDir, Settings.WildcardTldStatisticsDictFileName) )

       stopWatch.Stop()
       System.Console.WriteLine("collectDomainTldStatistics - Time elapsed: {0}", stopWatch.Elapsed)


       let stopWatch = System.Diagnostics.Stopwatch.StartNew()

       replace_digits_in_domaintld(Path.Combine(Settings.OutputTmpDir, Settings.WildcardTldStatisticsDictFileName),
                                   Path.Combine(Settings.OutputTmpDir, Settings.WildcardTldStatisticsReplDigitsDictFileName),
                                   Path.Combine(Settings.OutputTmpDir, Settings.WildcardTldStatisticsReplDigitsDictDistinctFileName))

       stopWatch.Stop()
       System.Console.WriteLine("replace_digits_in_domaintld - Time elapsed: {0}", stopWatch.Elapsed)


       let stopWatch = System.Diagnostics.Stopwatch.StartNew()
   
       processingOnePointDomainTldStatistics(Path.Combine(Settings.OutputTmpDir, Settings.SingleTldStatisticsCdIctFileName),
                                             Path.Combine(Settings.OutputTmpDir, Settings.SingleTldStatisticsCdIctProcessedFileName))
       
       stopWatch.Stop()
       System.Console.WriteLine("processingOnePointDomainTldStatistics - Time elapsed: {0}", stopWatch.Elapsed)

       let stopWatch = System.Diagnostics.Stopwatch.StartNew()

       replace_digits_in_domaintld( Path.Combine(Settings.OutputTmpDir, Settings.SingleTldStatisticsCdIctProcessedFileName),
                                    Path.Combine(Settings.OutputTmpDir, Settings.SingleTldStatisticsCdIctProcessedDoneFileName),
                                    Path.Combine(Settings.OutputTmpDir, Settings.SingleTldStatisticsCdIctProcessedDoneDistinctFileName) )

       stopWatch.Stop()
       System.Console.WriteLine("replace_digits_in_domaintld - Time elapsed: {0}", stopWatch.Elapsed)

       let stopWatch = System.Diagnostics.Stopwatch.StartNew()

       processingOnePointDomainTldStatisticsWithCount( Path.Combine(Settings.OutputTmpDir, Settings.OnePointDnsNamesFileName),
                                                       Path.Combine(Settings.OutputTmpDir, Settings.SingleOnePointTldStatisticsFileName) )

       stopWatch.Stop()
       System.Console.WriteLine("processingOnePointDomainTldStatisticsWithCount - Time elapsed: {0}", stopWatch.Elapsed)

       let stopWatch = System.Diagnostics.Stopwatch.StartNew()

       filterDnsNamesByNormalTld( Path.Combine(Settings.OutputTmpDir, Settings.OnePointDnsNamesFileName), 
                                  Path.Combine(Settings.OutputTmpDir, Settings.SingleOnePointNormalTldStatisticsFileName) )

       stopWatch.Stop()
       System.Console.WriteLine("filterDnsNamesByNormalTld - Time elapsed: {0}", stopWatch.Elapsed)

       let stopWatch = System.Diagnostics.Stopwatch.StartNew()

       filterDnsNamesByTld( Path.Combine(Settings.OutputTmpDir, Settings.OnePointDnsNamesFileName),  
                            Path.Combine(Settings.OutputTmpDir, Settings.SingleByTldStatisticsFileName) )

       stopWatch.Stop()
       System.Console.WriteLine("filterDnsNamesByTld - Time elapsed: {0}", stopWatch.Elapsed)

       startWatch.Stop()

       System.Console.WriteLine("Total Processing - Time elapsed: {0}", startWatch.Elapsed)

       let rec start_node = {    
         Name = "DNS root"  
         Parent = start_node
         Children = ConcurrentBag<TreeNode>()
       }

       [| Path.Combine(Settings.OutputResultDir, Settings.WildcardDnsResult);
          Path.Combine(Settings.OutputResultDir, Settings.WildcardDnsResultDetails);
          Path.Combine(Settings.OutputResultDir, Settings.SingleDnsResult);
          Path.Combine(Settings.OutputResultDir, Settings.WildcardDnsTreeResult);
          Path.Combine(Settings.OutputResultDir, Settings.SingleDnsTreeResult);
          Path.Combine(Settings.OutputResultDir, Settings.WildcardDnsTreeResultDetails);
          Path.Combine(Settings.OutputResultDir, Settings.SingleNodeTreeResult);
          Path.Combine(Settings.OutputResultDir, Settings.WildcardNodeTreeResult);
       |] 
       |> Seq.iter(fun f -> if File.Exists(f) then File.Delete(f))

       let lines = File.ReadAllLines(Path.Combine(Settings.OutputTmpDir, Settings.MultiPointDnsNamesFileName))
       let data = lines |> Seq.map revere_str |> Seq.sort |> Seq.map revere_str 

       printfn "Lines Count - %d" (Seq.length data)
       let countp = ref 0

       //let chunks = 100000
       let chunks = Seq.length data

       data 
       |> Seq.chunkBySize chunks |> Seq.iter(fun batch -> let dict_level_treenode_local = new ConcurrentDictionary<int, ConcurrentBag<TreeNode>>()
                                                          let dict_treenode = new ConcurrentDictionary<string, TreeNode>()

                                                          let stopWatch = System.Diagnostics.Stopwatch.StartNew()
                                                          
                                                          countp := !countp + 1
                                                          printfn "Batch size %d" (Seq.length batch)
                                                          

                                                          batch |> Seq.chunkBySize 10000 |> PSeq.iter (fun batchp -> batchp |> Array.Parallel.iter (fun line -> fastCreateTreeNodeLocal(line, start_node, dict_treenode, dict_level_treenode_local)))

                                                          stopWatch.Stop()
                                                          printfn "Time elapsed(TotalMilliseconds): %f" stopWatch.Elapsed.TotalMilliseconds
                                                          System.Console.WriteLine("Time elapsed: {0}", stopWatch.Elapsed)

                                                          let stopWatch = System.Diagnostics.Stopwatch.StartNew()

                                                          (*writeResultWildcardLocal(2, Path.Combine(Settings.OutputResultDir, Settings.WildcardDnsResult),
                                                                                        Path.Combine(Settings.OutputResultDir, Settings.WildcardDnsResultDetails), 
                                                                                        Path.Combine(Settings.OutputResultDir, Settings.SingleDnsResult),
                                                                                        dict_level_treenode_local)*)

                                                          stopWatch.Stop()
                                                          System.Console.WriteLine("writeResultWildcardLocal Time elapsed: {0}", stopWatch.Elapsed)

                                                          let stopWatch = System.Diagnostics.Stopwatch.StartNew()
                                                          //writeResultTreeLocal(Path.Combine(Settings.OutputResultDir, Settings.WildcardDnsTreeResult), Path.Combine(Settings.OutputResultDir, Settings.SingleDnsTreeResult), dict_level_treenode_local)
                                                          stopWatch.Stop()
                                                          System.Console.WriteLine("writeResultTree Time elapsed: {0}", stopWatch.Elapsed)

                                                          let stopWatch = System.Diagnostics.Stopwatch.StartNew()
                                                          collectSingleTreeNodes(Path.Combine(Settings.OutputResultDir, Settings.SingleNodeTreeResult), dict_level_treenode_local)
                                                          stopWatch.Stop()
                                                          System.Console.WriteLine("collectSingleTreeNodes Time elapsed: {0}", stopWatch.Elapsed)

                                                          let stopWatch = System.Diagnostics.Stopwatch.StartNew()
                                                          collectWildcardTreeNodesByLevel(Settings.OutputResultDir, dict_level_treenode_local)
                                                          stopWatch.Stop()
                                                          System.Console.WriteLine("collectWildcardTreeNodes Time elapsed: {0}", stopWatch.Elapsed)

                                                          let stopWatch = System.Diagnostics.Stopwatch.StartNew()
                                                          collectWildcardTreeNodes(Path.Combine(Settings.OutputResultDir, Settings.WildcardNodeTreeResult), dict_level_treenode_local)
                                                          stopWatch.Stop()
                                                          System.Console.WriteLine("collectWildcardTreeNodes Time elapsed: {0}", stopWatch.Elapsed)

                                                          dict_level_treenode_local.Clear()
                                                          dict_treenode.Clear()

                                                          replace_digits_in_domaintld(Path.Combine(Settings.OutputResultDir, Settings.CollectLevelTwoFileName),
                                                                                      Path.Combine(Settings.OutputResultDir, Settings.CollectLevelTwoResultFileName),
                                                                                      Path.Combine(Settings.OutputResultDir, Settings.CollectLevelTwoFinalResultFileName))


                                                          printfn "Summary size processed. - %d" ((Seq.length batch) * !countp)
                                                                ) 
       printfn "Finished." 
       Console.ReadLine() |> ignore
       0 // return an integer exit code
    with
      | _ as ex -> printfn "Error: %s" ex.Message
                   exit -1


    
