open System.IO
open System.Net
open System.Net.Http
open System
open System.Collections.Generic
open System.Text.RegularExpressions
open Nager.PublicSuffix
open System.Collections.Concurrent
open System.Threading.Tasks
open Microsoft.FSharp.Collections
open FSharp.Collections.ParallelSeq
open FSharp.Configuration

let getAsync (url:string) = 
    async {
        let handler = new HttpClientHandler()
        handler.AutomaticDecompression <- (DecompressionMethods.GZip ||| 
                                           DecompressionMethods.Deflate ||| 
                                           DecompressionMethods.Brotli)

        let client = new HttpClient(handler)
        let! response = client.GetAsync(url) |> Async.AwaitTask
        response.EnsureSuccessStatusCode () |> ignore
        let! content = response.Content.ReadAsStringAsync() |> Async.AwaitTask
        return content
    }

let downloadFileAsync (uri:string, output_file_path:string) = 
    async {
        let handler = new HttpClientHandler()
        handler.AutomaticDecompression <- (DecompressionMethods.GZip |||
                                           DecompressionMethods.Deflate ||| 
                                           DecompressionMethods.Brotli)

        use httpClient = new HttpClient(handler)
        let! fileBytes = httpClient.GetByteArrayAsync(uri) |> Async.AwaitTask
        File.WriteAllBytes(output_file_path, fileBytes)
    }

let dict_for_filter = new Dictionary<string, int>()
let resize = new ResizeArray<string>()
let mutable ignore_str = true

let filter_convert (str:string,start_covert_str:string,repl:string)= 
    if str.StartsWith(start_covert_str) then
       ignore_str <- false
    if not(ignore_str) then
        match str with
        | s when s.StartsWith("#", StringComparison.CurrentCultureIgnoreCase) -> resize.Add(s.Trim())
                                                                                 
        | _ as s -> let mutable key = ""
                    if not(String.IsNullOrWhiteSpace(repl)) then
                       key <- s.Trim().Replace(repl, String.Empty)
                    else
                       key <- s.Trim()

                    if not(dict_for_filter.ContainsKey(key)) then 
                        dict_for_filter.Add(key, 1)
                        resize.Add (key)

let strContainsOnlyNumber (s:string) = s |> Seq.forall Char.IsDigit



let filter (str:string)= 
    if not(dict_for_filter.ContainsKey(str)) && not(strContainsOnlyNumber(str.Replace(".","").Trim())) then 
        dict_for_filter.Add(str, 1)
        resize.Add(str)

let filter_porn (str:string)= 
    if dict_for_filter.ContainsKey(str) then 
        dict_for_filter.Remove(str) |> ignore

let download_processing_file (uri:string, outputpath:string, start_covert_str:string, repl:string, output_result_path:string) =

    if String.IsNullOrWhiteSpace(start_covert_str) then
       ignore_str <- false
    else
       ignore_str <- true

    downloadFileAsync(uri,outputpath) |> Async.RunSynchronously
    File.ReadLines(outputpath) |> Seq.iter(fun x -> filter_convert(x, start_covert_str, repl) |> ignore)

    if not(File.Exists(output_result_path)) then
       File.WriteAllLines(output_result_path, Seq.toArray resize)
    else
       use sw = File.AppendText(output_result_path)
       resize |> Seq.toArray |> Seq.iter(fun s ->  sw.WriteLine(s) |> ignore)
    
    dict_for_filter.Clear()
    resize.Clear()

let dict_filtered = new Dictionary<string, int>()



let filter_rules (str:string, repl:string) = 
    let tmps = str.Replace(repl, String.Empty).Trim()

    match tmps with
    | t when t.Contains("spot") || t.StartsWith("#") || Char.IsDigit t.[0] 
            || t.Contains("tumblr") || t.Contains("sex") || t.Contains("pussy") 
            || t.Contains("girl") || t.Contains("porn") || t.Contains("fuck") || 
             t.Contains("bab") || t.Contains("baby") || t.Contains("anal") || t.Contains("ass") || t.Contains("bitch") || t.Contains("back") || t.Contains("gay")
            || t.StartsWith("a") || t.StartsWith("bi") || t.StartsWith("black") || t.StartsWith("bad") || t.Contains("bdsm") 
            || t.Contains("adult") || t.Contains("tits") || t.Contains("mom") || t.Contains("teen") || t.Contains("blowjob") || t.Contains("brutal")
            || t.StartsWith("bai") || t.StartsWith("flirt") || t.StartsWith("hard") || t.StartsWith("bbw") || t.StartsWith("blue") || t.StartsWith("body") || t.StartsWith("bondage") || t.StartsWith("boy")
            || t.StartsWith("tranny") || t.StartsWith("trans") || t.StartsWith("travestie") || t.StartsWith("upskirt") || t.StartsWith("xxx") || t.StartsWith("mature")
            -> ()
    | _ as s -> if not(dict_filtered.ContainsKey(s)) then 
                   dict_filtered.Add(s, 1)
                   resize.Add(s)

let filter_rules_test (str:string) = 
    match str with
    | t when t.Contains("facebook") -> ()
    | n when n = ".com" -> printfn "%s" ".com"
    | _ as s -> if not(dict_filtered.ContainsKey(s)) then 
                   dict_filtered.Add(s, 1)
                   resize.Add(s)
       

let download_filter_processing_file (uri:string, outputpath:string, repl:string, output_result_path:string) =

    downloadFileAsync(uri,outputpath) |> Async.RunSynchronously
    File.ReadLines(outputpath) |> Seq.iter(fun x -> filter_rules(x, repl) |> ignore)

    if not(File.Exists(output_result_path)) then
       File.WriteAllLines(output_result_path, Seq.toArray resize)
    else
       use sw = File.AppendText(output_result_path)
       resize |> Seq.toArray |> Seq.iter(fun s ->  sw.WriteLine(s) |> ignore)
    
    dict_for_filter.Clear()
    resize.Clear()

    
 /// Gets all combinations (without repetition) of specified length from a list.
let rec getCombs n lst = 
    match n, lst with
    | 0, _ -> seq [[]]
    | _, [] -> seq []
    | k, (x :: xs) -> Seq.append (Seq.map ((@) [x]) (getCombs (k - 1) xs)) (getCombs k xs)
     
let allCombinations lst =
    let rec comb accLst elemLst =
        match elemLst with
        | h::t ->
            let next = [h]::List.map (fun el -> h::el) accLst @ accLst
            comb next t
        | _ -> accLst
    comb [] lst

let dict_memoization_data = new Dictionary<string, ResizeArray<string>>()

(*let longest_common_substring (first_str:string, second_str:string) =
    let mutable substrings = new ResizeArray<string>()

    if dict_memoization_data.ContainsKey(first_str) then 
       substrings <- dict_memoization_data[first_str]
    else
        for i = 0 to first_str.Length - 1 do
          for j = 1 to (first_str.Length - i) do
             substrings.Add(first_str.Substring(i, j))

        dict_memoization_data.Add(first_str,substrings)

    substrings |> Seq.sortBy(fun s -> s.Length) 
               |> Seq.rev 
               |> Seq.tryFind(fun x -> second_str.Contains(x))
               |> function 
                  | Some(t) -> t 
                  | None -> String.Empty *)

                  
let count_chr x xs =  xs
                      |> Seq.filter (fun x' -> x' = x)
                      |> Seq.length


let longest_common_substring (first_str:string, second_str:string) =
    let substrings = new ResizeArray<string>()

    for i = 0 to first_str.Length - 1 do
      for j = 1 to (first_str.Length - i) do
         substrings.Add(first_str.Substring(i, j))

    substrings |> Seq.sortBy(fun s -> s.Length) 
               |> Seq.rev 
               |> Seq.tryFind(fun x -> second_str.Contains(x))
               |> function 
                  | Some(t) -> let mutable result = t
                               if count_chr '.' t >= 1 then
                                  let index = t.IndexOf('.')
                                  if index > 0 && index < t.Length then
                                     result <- t.Substring(index, t.Length - index)
                               new string(result)
                  | None -> String.Empty

let revere_str (s:string) = s |> Array.ofSeq |> Array.rev |> System.String

let charCountFrom str chr i = 
    let rec loop = function
        | (it, count) when (String.length str) = it -> count
        | (it, count) when str.[it] = chr -> loop (it+1, count+1)
        | (it, count) -> loop (it+1, count)
    loop (i,0)


let find_first_dif_char_index (f:string) (s:string) =
    Seq.zip f s |> Seq.takeWhile (fun (c1, c2) -> c1 = c2) |> Seq.length


let levenshteinDistance (source : string) (target : string) =
  let wordLengths = (source.Length, target.Length)
  let matrix = Array2D.create ((fst wordLengths) + 1) ((snd wordLengths) + 1) 0
 
  for c1 = 0 to (fst wordLengths) do
    for c2 = 0 to (snd wordLengths) do
      matrix.[c1, c2] <-
        match (c1, c2) with
        | h, 0 -> h
        | 0, w -> w
        | h, w ->
          let sourceChar, targetChar = source.[h - 1], target.[w - 1]
          let cost = if sourceChar = targetChar then 0 else 1
          let insertion = matrix.[h, w - 1] + 1
          let deletion = matrix.[h - 1, w] + 1
          let subst = matrix.[h - 1, w - 1] + cost
          Math.Min(insertion, Math.Min(deletion, subst))
  matrix.[fst wordLengths, snd wordLengths]

let dict_collected_data = new Dictionary<string, ResizeArray<string * string>>()

let rem_comment(str:string) : string =
    let tstr = str.Trim()
    let indx = tstr.IndexOf("#")
    if tstr.Contains("#") && indx > 0 then
       tstr.Substring(0,indx - 1)
    else
       tstr

let is_valid_ipv4 (str:string) =
    let (isValidIp,_) = System.Net.IPAddress.TryParse(str)
    isValidIp


let filter_www (str:string) = 
    match str with
    | st when st.StartsWith("www.", StringComparison.CurrentCultureIgnoreCase) -> let key = st.Replace("www.",String.Empty).Trim()
                                                                                  if not(dict_filtered.ContainsKey(key)) then 
                                                                                      dict_filtered.Add(key, 1)
                                                                                      resize.Add (key)
    | tx when tx.StartsWith("#", StringComparison.CurrentCultureIgnoreCase) -> resize.Add(tx.Trim())
    | _ as txt ->  let text = txt.Trim()
                   if (not(text.Contains("/")) && not(is_valid_ipv4(text))) && not(dict_filtered.ContainsKey(text)) then 
                     dict_filtered.Add(text, 1)
                     resize.Add(text)

let filter_www_trim_dns (input:string) (output:string) = 
    let lines = File.ReadLines(input)
    lines |> Seq.iter(fun x -> filter_www (rem_comment(x)))
    File.WriteAllLines(output, (Seq.toArray dict_filtered.Keys))
    dict_filtered.Clear()

let isstr_contains_digits (str:string) = 
    str |> Seq.filter(fun c-> Char.IsDigit c) |> (not << Seq.isEmpty)

let ld x = Math.Log x / Math.Log 2.

let entropy (s : string) =
   let n = float s.Length
   Seq.groupBy id s
   |> Seq.map (fun (_, vals) -> float (Seq.length vals) / n)
   |> Seq.fold (fun e p -> e - p * ld p) 0. 

let digits_count_in_str (str:string) = 
    str |> Seq.filter(fun c-> Char.IsDigit c) |> Seq.distinct |> Seq.length

let relative_correlation_entropy (str:string) =
    let h = (entropy str)
    let real_count_char = str |> Seq.filter(fun c-> not(Char.IsDigit c)) |> Seq.distinct |> Seq.length
    let hrmax = Math.Log(real_count_char + digits_count_in_str(str) |> float, 2.)
    (h/hrmax)

let relative_entropy (str:string) =
    let h = (entropy str)
    let hmax = Math.Log(26 + digits_count_in_str(str) |> float, 2.)
    (h/hmax)

let analyze_dns (fdns:string, sdns:string) = 
    let search = '.';
    let fdata = fdns |> Seq.mapi(fun index x  -> {| Index = index; Char = x |}) |> Seq.filter (fun r -> r.Char = search) 
    let sdata = sdns |> Seq.mapi(fun index x  -> {| Index = index; Char = x |}) |> Seq.filter (fun r -> r.Char = search) 

    let isEqual = (fdata, sdata) ||> Seq.forall2 (=)
    if isEqual then 
       let ar = fdns.Split('.')
       let az = sdns.Split('.')
       let t = Seq.zip ar az
       let datat = t |> Seq.map(fun (x,y) -> (x, y, x=y) )
       //datat |> Seq.iter(fun (x,y, z) -> printfn "%s %s %b" x y z )
       //let firstd = datat |> Seq.take 1 |> Seq.map(fun (x, y, z) -> if z && isstr_containse_digits x then ("*", "*", z) else (x, y, z) )
       //let lastd = datat |> Seq.skip 1
       //let datat = Seq.concat [firstd;lastd] 

       let res = ('.', datat |> Seq.map(fun (x, _, z) -> if not(z) || isstr_contains_digits x then "*" else x )) |> System.String.Join
       res
    else
       String.Empty

let replace_random_strwithdigits_in_dnsnames (dns:string) = 
    let search = '.';
    let dnsline = rem_comment(dns)
    let fdata = dnsline |> Seq.mapi(fun index x  -> {| Index = index; Char = x |}) |> Seq.filter (fun r -> r.Char = search) 
    if (count_chr search dnsline) >= 2 then
       let ar = dnsline.Split('.')
       let re = (relative_entropy ar.[0])
       ('.', ar |> Seq.map(fun (x) -> if ( (isstr_contains_digits x) && ((relative_correlation_entropy x) > 0.9) && (x.Length >=6) ) || x.Contains("-") then "*" else x )) |> System.String.Join //
    else
       dnsline

let collect_data_processing (lines: IEnumerable<string>) = 
   
    let dict = new Dictionary<string, ResizeArray<string * string>>()
    let wildcard = "*"

    let flines = lines |> Seq.toArray 
    let slines = lines |> Seq.toArray 

    let mutable i = 0
    while i < (flines |> Seq.length) - 1 do
       let mutable breakl = false

       let fline = flines.[i]
       let mutable ftline:string = fline

       if not(dict.ContainsKey(fline)) && not(fline.StartsWith("#", StringComparison.CurrentCultureIgnoreCase)) then
          let list = new ResizeArray<string * string>()
          list.Add((fline,"")) 
          dict.Add(fline, list)   
       else
          i <- i + 1
          breakl <- true

       let mutable j = i + 1
       
       while j < (slines |> Seq.length) - 1 && not(breakl) do
          let sline = slines.[j]
          if (count_chr '.' fline) = (count_chr '.' sline) then
              let mutable stline:string = sline

              let commont_str = longest_common_substring(ftline, stline)

              let ar = stline.Split('.')

              let ar0 = new string(ar.[0])
              System.Array.Clear(ar, 0, ar.Length)

              if stline.StartsWith(commont_str) && isstr_contains_digits ar0 then
                 ftline <- ftline.Replace(ar0, wildcard)
                 stline <- stline.Replace(ar0, wildcard)

              let ld  = levenshteinDistance ftline stline
              let commont_str = longest_common_substring(ftline, stline)

              if (ftline.Length = stline.Length && commont_str.Length > 0) then 
                 let template = analyze_dns(ftline, stline)
                 if not(template = String.Empty) then 
                    let list = dict[fline]
                    list.Add((stline, template))

              if ld <= Seq.min [ftline.Length; stline.Length] && ((ftline.Length > 10 && commont_str.Length >= 8) || (ftline.Length <= 8 && commont_str.Length >= 3)) then
                 let list = dict[fline]
                 let (_, common) = list |> Seq.last
                 if commont_str.Length < common.Length then
                    i <- j
                    breakl <- true
                 else
                     list.Add((stline,commont_str))
                     j <- j + 1
              else
                 i <- j
                 breakl <- true
          else
            i <- j + 1
            breakl <- true
    dict
    
let collect_randomdata_processing (lines: IEnumerable<string>) = 
   
    let dict = new Dictionary<string, int>()
    let flines = lines |> Seq.toArray 

    let mutable i = 0
    while i < (flines |> Seq.length) - 1 do
       let mutable breakl = false

       let fline = flines.[i]

       if not(dict.ContainsKey(fline)) then
          dict.Add(fline, 1)   
       else
          i <- i + 1
          breakl <- true

       let mutable j = i + 1
       
       while j < (flines |> Seq.length) - 1 && not(breakl) do
          let sline = flines.[j]
          if (count_chr '.' fline) = (count_chr '.' sline) then
              let template = analyze_dns(fline, sline)
              if not(template = String.Empty) && not(dict.ContainsKey(template)) then 
                 dict.Add(template, 1)
              j <- j + 1
          else
            i <- j + 1
            breakl <- true
    dict 

let filter_ (str:string) = 
    match str with
    | t when t.StartsWith("#") -> ()
    | n when n = "" -> ()
    | t when Char.IsDigit t.[0] 
            || t.Contains("-") || t.Contains("2o7.net")
            || t.Contains("facebook") || t.Contains("whatsapp") || t.Contains("appspot")
            -> ()
    | _ as s -> if not(dict_filtered.ContainsKey(s)) then 
                   dict_filtered.Add(s, 1)
                   resize.Add(s)

let filter_res (str:string) = 
    match str with
    | t when t.StartsWith("#") -> ()
    | n when n = "" -> ()
    | n when n.Length <= 4 && n.Contains(".") -> let rs = "#" + n
                                                 resize.Add(rs)
    | _ as s -> if not(dict_filtered.ContainsKey(s)) then 
                   dict_filtered.Add(s, 1)


let Dispose(sw:StreamWriter) =
    sw.Flush()
    sw.Close()
    sw.Dispose()

let replace_random_str_in_dnsnames(input:string, poutput:string, foutput:string, woutput:string) = 

    let dict_filtered = new Dictionary<string, int>()

    let lines = File.ReadAllLines(input) 
    let data = lines |> Seq.map(fun line -> replace_random_strwithdigits_in_dnsnames line)

    File.WriteAllLines(foutput, data) 
    
    let lines = File.ReadAllLines(foutput)

    use swp = File.AppendText(poutput) //
    swp.AutoFlush <- true

    lines |> Seq.iter(fun l -> if l.Contains("*") then
                                  if l.Length > 6 && not(dict_filtered.ContainsKey(l)) then
                                     dict_filtered.Add(l,1)
                               else swp.WriteLine(l))

    File.WriteAllLines(woutput, dict_filtered.Keys) 

    dict_filtered.Clear()

    Dispose(swp)


let wildcard_processing(dict:Dictionary<string, ResizeArray<string * string>>, woutput:string, soutput:string, routput:string) = 
    let wildcard_dict_data = new Dictionary<string, string>()
    let single_data = new ResizeArray<string>()
    
    use swc = File.AppendText(woutput)
    swc.AutoFlush <- true
    use sws = File.AppendText(soutput)
    sws.AutoFlush <- true

    for k in dict.Keys do
        let data = dict[k] 

        let mutable wildcard = ""
        let common_str = new ResizeArray<string * string * string>()
        if data.Count >= 2 then
           swc.WriteLine("# Key - " + k)
           data |> Seq.skip(1) |> Seq.iter(fun (s,c) -> if c.Contains("*") then 
                                                           if not(wildcard_dict_data.ContainsKey(c)) then 
                                                              wildcard_dict_data.Add(c, "")
                                                        else
                                                            if s.EndsWith(c) then wildcard <- "*" + c  
                                                            if s.StartsWith(c) then wildcard <- c + "*"
                                                            if not(s.StartsWith(c)) && not(s.EndsWith(c)) then wildcard <- "*" + c + "*"

                                                            if s = c then wildcard <- ""
                                                            common_str.Add((s, c , wildcard)) 

                                                            if not(s.StartsWith(c)) && not(s.EndsWith(c)) && not(wildcard_dict_data.ContainsKey(wildcard)) then
                                                               wildcard_dict_data.Add(wildcard, "")
                                                            else
                                                                let mutable added = false

                                                                let indxl = wildcard.LastIndexOf(".")
                                                                if indxl > 0 && wildcard.Length - indxl <= 10 && wildcard.EndsWith("*") && not(wildcard.[wildcard.Length - 2] = '.') then
                                                                   let wcard = wildcard.Substring(0, indxl+1) + "*"

                                                                   if not(wildcard_dict_data.ContainsKey(wcard)) then
                                                                       wildcard_dict_data.Add(wcard, "")
                                                                       added <- true
                                                                   else
                                                                       added <- true

                                                                let index = wildcard.IndexOf(".")
                                                                let chr_count = count_chr '.' wildcard
                                                                if chr_count > 1 && index > 0 && index <= 10 && wildcard.[0] = '*' && not(wildcard.[1] = '.') then
                                                                   let wcard = "*" + wildcard.Substring(index, wildcard.Length - index)
                                                        
                                                                   if not(wildcard_dict_data.ContainsKey(wcard)) then
                                                                       wildcard_dict_data.Add(wcard, "")
                                                                       added <- true
                                                                   else
                                                                       added <- true

                                                                else
                                                                   if not(added) && not(wildcard_dict_data.ContainsKey(wildcard)) then
                                                                       wildcard_dict_data.Add(wildcard, "")
                                           )
           
           common_str |> Seq.iter(fun (s, c, w) -> swc.WriteLine(sprintf "%s - %s - %s" s c w))
        else
           single_data.Add(k)
           //sws.WriteLine(k)
           
    swc.Flush()
    swc.Close()

    single_data |> Seq.map revere_str |> Seq.sort |> Seq.map revere_str |> Seq.iter (fun s -> sws.WriteLine(s)) 

    sws.Flush()
    sws.Close()

    single_data.Clear()

    use swr = File.AppendText(routput)
    swr.AutoFlush <- true
    wildcard_dict_data.Keys |> Seq.iter(fun s -> swr.WriteLine(s))
    swr.Flush()
    swr.Close()
    wildcard_dict_data.Clear()

let is_valid_domain_name(name:string) =
    not(Uri.CheckHostName(name) = UriHostNameType.Unknown)

let is_str_wildcard_empty(str:string) =
    let validator = new Regex(@"^[*.]+$")
    let index = str.LastIndexOf(".")
    if index > 0 then 
       let sstr = str.Substring(0, index + 1)
       validator.IsMatch(sstr) 
    else
       validator.IsMatch(str)

let dispose_sw(sw:StreamWriter) =
    sw.Flush()
    sw.Close()
    sw.Dispose()

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
                                  let data = splitted |> Seq.map(fun x -> let output = Regex.Replace(x, @"\d{2,}", "@")
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

let clear_dictionary() =
    dict_memoization_data.Clear()
    dict_collected_data.Clear()
    dict_filtered.Clear()
    resize.Clear()

type Settings = AppSettings<"app.config">
                 
[<EntryPoint>]
let main _ =
    printfn "Start Processing blocklist dnsnames file." 

    try
        clear_dictionary()

        let stopWatch = System.Diagnostics.Stopwatch.StartNew()

        [| Path.Combine(Settings.InputDataSourceRootDir, Settings.OutputFilteredDataSourceFileName);
           Path.Combine(Settings.InputDataSourceRootDir, Settings.ProcessingRandomStrFileName);
           Path.Combine(Settings.InputDataSourceRootDir, Settings.RemoveRandomStrFileName);
           Path.Combine(Settings.InputDataSourceRootDir, Settings.WildcardRandomFileName);
           Path.Combine(Settings.InputDataSourceRootDir, Settings.DnsFilteredFileName);
           Path.Combine(Settings.InputDataSourceRootDir, Settings.DnsWildcardFileName);
           Path.Combine(Settings.InputDataSourceRootDir, Settings.DnsSingleFilteredFileName);
           Path.Combine(Settings.InputDataSourceRootDir, Settings.DnsWildcardDistinctFileName);
           Path.Combine(Settings.InputDataSourceRootDir, Settings.OnePointDnsFilteredFileName);
           Path.Combine(Settings.InputDataSourceRootDir, Settings.MultiPointDnsFilteredFileName);
           Path.Combine(Settings.InputDataSourceRootDir, Settings.WildcardRandomDnsResultFileName);
           Path.Combine(Settings.InputDataSourceRootDir, Settings.WildcardRandomDnsResultFilteredFileName);
           Path.Combine(Settings.InputDataSourceRootDir, Settings.WildcardRandomDnsResultReplDigitsFileName);
           Path.Combine(Settings.InputDataSourceRootDir, Settings.WildcardRandomDnsFinalResultReplDigitsFileName);
        |] 
        |> Seq.iter(fun f -> if File.Exists(f) then File.Delete(f))

        filter_www_trim_dns (Path.Combine(Settings.InputDataSourceRootDir, Settings.InputDataSourceFileName)) 
                            (Path.Combine(Settings.InputDataSourceRootDir, Settings.OutputFilteredDataSourceFileName))

        clear_dictionary()
    
    
        let filter_dnsonepoint (str:string) = 
            match str with
            | t when count_chr '.' t = 1 -> resize.Add(t)
            | _ as s -> if not(dict_filtered.ContainsKey(s)) then 
                           dict_filtered.Add(s, 1)

        let lines = File.ReadAllLines(Path.Combine(Settings.InputDataSourceRootDir, Settings.OutputFilteredDataSourceFileName))
        lines |> Seq.iter(fun x -> filter_dnsonepoint(x) |> ignore) 

        File.WriteAllLines(Path.Combine(Settings.InputDataSourceRootDir, Settings.OnePointDnsFilteredFileName)  , resize)
        File.WriteAllLines(Path.Combine(Settings.InputDataSourceRootDir, Settings.MultiPointDnsFilteredFileName), dict_filtered.Keys)

        replace_random_str_in_dnsnames(Path.Combine(Settings.InputDataSourceRootDir, Settings.MultiPointDnsFilteredFileName),
                                       Path.Combine(Settings.InputDataSourceRootDir, Settings.ProcessingRandomStrFileName),
                                       Path.Combine(Settings.InputDataSourceRootDir, Settings.RemoveRandomStrFileName), 
                                       Path.Combine(Settings.InputDataSourceRootDir, Settings.WildcardRandomFileName))
     
        clear_dictionary() 

    
        let lines = File.ReadAllLines(Path.Combine(Settings.InputDataSourceRootDir, Settings.WildcardRandomFileName))

        let resdict = collect_randomdata_processing(lines)
        File.WriteAllLines(Path.Combine(Settings.InputDataSourceRootDir, Settings.WildcardRandomDnsResultFileName), resdict.Keys)

        let filter (str:string) = 
            match str with
            | _ as s -> let mutable key = s.Replace("..",".*.")
                        if s.StartsWith("*") then 
                           key <- "*." + s.TrimStart [|'*';'.'|]
                        if s.StartsWith(".") then 
                           key <- "*." + s
                        if not(is_str_wildcard_empty(key)) && not(dict_filtered.ContainsKey(key)) then 
                           dict_filtered.Add(key, 1)

        let lines = File.ReadAllLines(Path.Combine(Settings.InputDataSourceRootDir, Settings.WildcardRandomDnsResultFileName))
        lines |> Seq.iter(fun x -> filter(x) |> ignore) 
    
    
        File.WriteAllLines(Path.Combine(Settings.InputDataSourceRootDir, Settings.WildcardRandomDnsResultFilteredFileName), dict_filtered.Keys)

        replace_digits_in_domaintld(Path.Combine(Settings.InputDataSourceRootDir, Settings.WildcardRandomDnsResultFilteredFileName),
                                    Path.Combine(Settings.InputDataSourceRootDir, Settings.WildcardRandomDnsResultReplDigitsFileName),
                                    Path.Combine(Settings.OutputResultDir, Settings.WildcardRandomDnsFinalResultReplDigitsFileName) 
                                   )

        stopWatch.Stop()
        System.Console.WriteLine("Random string in dnsnames replace - Time elapsed: {0}", stopWatch.Elapsed)
    with
    | _ as ex -> printfn "Error: %s" ex.Message

    try
        let stopWatch = System.Diagnostics.Stopwatch.StartNew()

        let lines = File.ReadAllLines(Path.Combine(Settings.InputDataSourceRootDir, Settings.ProcessingRandomStrFileName))

        printfn "Number of lines: %d" (lines |> Seq.length)
        
        let chunked = lines |> Seq.map revere_str |> Seq.sort |> Seq.map revere_str |> Seq.chunkBySize 1500000

        let write_collected_data_to_file (output:string, dict:Dictionary<string, ResizeArray<string * string>>) =
            use sw = File.AppendText(output)
            sw.AutoFlush <- true

            for k in dict.Keys do
                sw.WriteLine("# Key - " + k)
                let data = dict[k] 
                data |> Seq.iter(fun (s,c) -> sw.WriteLine(sprintf "%s - %s" s c))
            
            sw.Flush()
            sw.Close()

        let rec removeAll (map:Map<_,_>,  keys:list<_>) =
            match keys with
                | [] -> map
                | key :: rest -> removeAll(map.Remove(key), rest)

        chunked |> Seq.iter(fun batch ->  printfn "Processing batch size - %d" batch.Length

                                          let dict_collected = collect_data_processing(batch) 
                                         
                                          write_collected_data_to_file(Path.Combine(Settings.InputDataSourceRootDir, Settings.DnsFilteredFileName), dict_collected)
                                                                              
                                          wildcard_processing(dict_collected, Path.Combine(Settings.InputDataSourceRootDir, Settings.DnsWildcardFileName), 
                                                                              Path.Combine(Settings.InputDataSourceRootDir, Settings.DnsSingleFilteredFileName), 
                                                                              Path.Combine(Settings.OutputResultDir, Settings.DnsWildcardDistinctFileName))

                                          dict_memoization_data.Clear()
                                          dict_collected.Clear()

                                         )
        clear_dictionary()

        stopWatch.Stop()
        System.Console.WriteLine("Wildcard processing - Time elapsed: {0}", stopWatch.Elapsed)
     with
     | :? System.OutOfMemoryException as ex -> printfn "Error: OutOfMemoryException %s" ex.Message
     | _ as ex -> printfn "Error: %s" ex.Message

    printfn "Finished."
    Console.ReadLine() |> ignore
    0 // return an integer exit code
