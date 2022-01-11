open System.IO
open System.Net
open System.Net.Http
open System
open System.Collections.Generic
open SharpCompress.Readers;
open FSharp.Configuration

let downloadFileAsync (uri:string, output_file_path:string) = 
    async {
        let handler = new HttpClientHandler()
        handler.AutomaticDecompression <- (DecompressionMethods.GZip |||
                                           DecompressionMethods.Deflate ||| 
                                           DecompressionMethods.Brotli)

        use httpClient = new HttpClient(handler)
        let! fileBytes = httpClient.GetByteArrayAsync(uri) |> Async.AwaitTask
        File.WriteAllBytes(output_file_path, fileBytes); 
    }

let downloadStreamFileAsync (uri:string, output_file_path:string) = 
    async {
        let handler = new HttpClientHandler()
        handler.AutomaticDecompression <- (DecompressionMethods.GZip |||
                                           DecompressionMethods.Deflate ||| 
                                           DecompressionMethods.Brotli)

        use httpClient = new HttpClient(handler)
        use file = File.OpenWrite(output_file_path)
        let! response = httpClient.GetStreamAsync(uri) |> Async.AwaitTask
        do! response.CopyToAsync(file) |> Async.AwaitTask
    }

let rem_comment(str:string) : string =
    let indx = str.IndexOf("#")
    if str.Contains("#") && indx > 0 then
       str.Substring(0, indx - 1).Trim()
    else
       str.Trim()

let download_file (uri:string, outputpath:string, output_result_path:string) =

    let dict_for_filter = new Dictionary<string, int>()
    let resize = new ResizeArray<string>()

    let filter (str:string)= 
        match str with
        | s when s.StartsWith("#", StringComparison.CurrentCultureIgnoreCase) -> resize.Add(s.Trim())
        | _ as s -> let key = rem_comment(s.Trim())
                    if not(dict_for_filter.ContainsKey(key)) then 
                       dict_for_filter.Add(key, 1)
                       resize.Add(s.Trim())

    try
       try
           printfn "download_file url: %s filename: %s" uri outputpath

           downloadFileAsync(uri,outputpath) |> Async.RunSynchronously
           File.ReadLines(outputpath) |> Seq.iter(fun x -> filter(x) |> ignore)

           if not(File.Exists(output_result_path)) then
              File.WriteAllLines(output_result_path, Seq.toArray dict_for_filter.Keys)
           else
              use sw = File.AppendText(output_result_path)
              dict_for_filter.Keys |> Seq.toArray |> Seq.iter(fun s ->  sw.WriteLine(s.Trim()) |> ignore)
       
       with
       | _ as ex -> printfn "download_file Error: %s on url %s" ex.Message uri

    finally
       dict_for_filter.Clear()
       resize.Clear()
    

let download_archive(uri:string, outputpath:string) =
    try
       printfn "download_archive url: %s filename: %s" uri outputpath
       downloadFileAsync(uri,outputpath) |> Async.RunSynchronously
    with
    | _ as ex -> printfn "download_archive Error: %s on url %s" ex.Message uri
    

let processing_dat_file (root_dir:string,sourcepath:string, output_result_path:string) =

    let dict_for_filter = new Dictionary<string, int>()
    let resize = new ResizeArray<string>()

    let fmap funcx list =
        [for elem in list -> funcx elem]

    let filter (str:string)= 
        match str with
        | s when s.StartsWith("#", StringComparison.CurrentCultureIgnoreCase) -> ()
        | _ as s -> let key = rem_comment(s.Trim())
                    let splited = key.Split(',')
                    let sval = splited.[0]
                    let ipsplited = sval.Split('-')
                    let fip = ipsplited.[0].Trim()
                    let sip = ipsplited.[1].Trim()
                    let first_ip = fip.Split('.')
                    let ifrom = first_ip.[2].Trim() |> int
                    let second_ip = sip.Split('.') 
                    let ito = second_ip.[2].Trim() |> int
                    let jfrom = first_ip.[3].Trim() |> int
                    let jto = second_ip.[3].Trim() |> int

                    if not(first_ip.[2].Trim() = second_ip.[2].Trim()) then
                        resize.Add(s.Trim())

                    for i=ifrom to ito do
                    for j=jfrom to jto do
                        let template =  (".", first_ip |> Seq.take 2 |> fmap (fun x -> if x.TrimStart('0') = String.Empty then "0" else x.TrimStart('0'))) |> String.Join  
                        let key = template + "." + (string i) + "." + (string j) 
                        if not(dict_for_filter.ContainsKey(key)) then 
                           dict_for_filter.Add(key, 1)

    File.ReadLines(sourcepath) |> Seq.iter(fun x -> filter(x) |> ignore)

    if not(File.Exists(output_result_path)) then
       File.WriteAllLines(output_result_path, Seq.toArray dict_for_filter.Keys)
       File.WriteAllLines(Path.Combine(root_dir,"combined-final-windat-pd.txt"), Seq.toArray resize)
    else
       use sw = File.AppendText(output_result_path)
       dict_for_filter.Keys |> Seq.toArray |> Seq.iter(fun s ->  sw.WriteLine(s.Trim()) |> ignore)
    
    dict_for_filter.Clear()
    resize.Clear()

let extract_tar_gz(path_to_tgz_arhive:string, dest_folder:string) =
    use stream = File.OpenRead(path_to_tgz_arhive)
    let reader = ReaderFactory.Open(stream)
    while reader.MoveToNextEntry() do
       if not(reader.Entry.IsDirectory) then 
          reader.WriteEntryToDirectory(dest_folder)

type Settings = AppSettings<"app.config">

let checkDirStructure(root_dir:string, output_result_dir:string,output_result_path:string) =
    let root_dir_arch = Settings.RootDirArch

    if not(Directory.Exists(root_dir)) then
       Directory.CreateDirectory(root_dir) |> ignore
    else
       let files = Directory.GetFiles(root_dir)
       files |> Seq.iter(fun f -> File.Delete(f))

    if not(Directory.Exists(output_result_dir)) then
        Directory.CreateDirectory(output_result_dir) |> ignore
     else
        let files = Directory.GetFiles(output_result_dir)
        files |> Seq.iter(fun f -> //File.Move(f, Path.Combine(root_dir_arch, Path.GetFileName(f)))
                                   if File.Exists(f) then
                                      File.Delete(f))

    if File.Exists(output_result_path) then
       File.Move(output_result_path, Path.Combine(root_dir_arch, Path.GetFileName(output_result_path)))
       File.Delete(output_result_path)

let DownloadIPAddressDataFiles() =
    let root_dir = Settings.DownloadTmpRootDir
    let output_result_dir = Settings.OutputResultDir
    let config_dir = Settings.ConfigDir
    let output_result_path = Path.Combine(Settings.OutputResultDir, Settings.ResultFileName)

    let trimCharAr = [|'"'|]
    
    let lines = File.ReadAllLines(Path.Combine(config_dir, Settings.IplInksDataFileInConfigDir)) 

    lines 
    |> Seq.iter(fun line -> let splitted = line.Split("|")
                            let url = splitted.[0].Trim(trimCharAr)
                            let filename = splitted.[1].Trim(trimCharAr)
                            let fext = Path.GetExtension(filename)
                            match fext with
                            | ".gz" -> download_archive(url, Path.Combine(root_dir, filename)) 
                            | _ -> download_file(url, Path.Combine(root_dir, filename), output_result_path)
                )

    let tgzpath_to_files = Directory.GetFiles(root_dir,  Settings.ArchiveFilePatternExt)
    tgzpath_to_files |> Seq.iter( fun path_to_file -> extract_tar_gz(path_to_file, root_dir))

    let extFile = Settings.DownloadFilePatternExt
    let path_to_files = Directory.GetFiles(root_dir, extFile)

    let dict_for_filter = new Dictionary<string, int>()
    
    let filter (str:string)= 
        match str with
        | s when s.StartsWith("#", StringComparison.CurrentCultureIgnoreCase) -> ()
        | _ as s -> let key = rem_comment(s.Trim())
                    if not(dict_for_filter.ContainsKey(key)) then 
                       dict_for_filter.Add(key, 1)
    
    path_to_files |> Seq.iter( fun f -> let lines = File.ReadLines(f) 
                                        lines |> Seq.iter(fun x -> filter(x) |> ignore) )
    
    File.WriteAllLines(Path.Combine(output_result_dir, Settings.ResultFileName), (Seq.toArray dict_for_filter.Keys))
    
    dict_for_filter.Clear()

[<EntryPoint>]
let main _ =
    try
       checkDirStructure(Settings.DownloadTmpRootDir, Settings.OutputResultDir, Path.Combine(Settings.OutputResultDir, Settings.ResultFileName))

       printfn "Start download files."
   
       let stopWatch = System.Diagnostics.Stopwatch.StartNew()
    
       DownloadIPAddressDataFiles()
    
       stopWatch.Stop()
       System.Console.WriteLine("Download and Preprocessing File Time elapsed: {0}", stopWatch.Elapsed)
   
       printfn "Finished."

       Console.ReadLine() |> ignore
       exit 0
    with
    | _ as ex -> printfn "Error: %s" ex.Message
                 exit -1


