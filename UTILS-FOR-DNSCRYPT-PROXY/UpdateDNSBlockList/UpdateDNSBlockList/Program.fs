// Learn more about F# at http://fsharp.org

open System
open System.IO
open System.Net
open System.Net.Http
open System.Collections.Generic
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
        /// copy the response contents to the file asynchronously
        do! response.CopyToAsync(file) |> Async.AwaitTask
    }

let dict_for_filter = new Dictionary<string, int>()
let mutable ignore_str = true

let filter_convert (str:string,start_covert_str:string,repl:string)= 
    if str.StartsWith(start_covert_str) then
       ignore_str <- false
    if not(ignore_str) then
        match str with
        | s when s.StartsWith("#", StringComparison.CurrentCultureIgnoreCase) -> ()
        | s when String.IsNullOrWhiteSpace(s) -> ()
        | _ as s -> let mutable key = ""
                    if not(String.IsNullOrWhiteSpace(repl)) then
                       key <- s.Trim().Replace(repl, String.Empty)
                    else
                       key <- s.Trim()

                    if not(dict_for_filter.ContainsKey(key)) then 
                        dict_for_filter.Add(key, 1)


let download_processing_file (uri:string, outputpath:string, start_covert_str:string, repl:string) =
    try

       if String.IsNullOrWhiteSpace(start_covert_str) then
          ignore_str <- false
       else
          ignore_str <- true

       downloadFileAsync(uri,outputpath) |> Async.RunSynchronously
       File.ReadAllLines(outputpath) |> Seq.iter(fun x -> filter_convert(x, start_covert_str, repl) |> ignore)

    with
    | :? AggregateException as ex -> printfn "Error Url - %s : %s" uri ex.Message
    | :? WebException as ex -> printfn "Error Url - %s : %s" uri ex.Message
    | _ as ex -> printfn "Error Url - %s : %s" uri ex.Message

let time f x = System.Diagnostics.Stopwatch.StartNew() |> (fun sw -> (f x, sw.Elapsed))

type Settings = AppSettings<"app.config">    

let checkDirStructure() =
    if String.IsNullOrEmpty(Settings.OutputDownloadFilesDir) then
       printfn "Parameter in app.config OutputDownloadFilesDir can't be empty."
    if String.IsNullOrEmpty(Settings.ConfigDir) then
       printfn "Parameter in app.config ConfigDir can't be empty."
    if String.IsNullOrEmpty(Settings.LinksDataFileInConfigDir) then
       printfn "Parameter in app.config LinksDataFileInConfigDir can't be empty."
    if String.IsNullOrEmpty(Settings.BlockFilePrefix) then
       printfn "Parameter in app.config BlockFilePrefix can't be empty."
    if String.IsNullOrEmpty(Settings.OutputResultDir) then
        printfn "Parameter in app.config OutputResultDir can't be empty."
    if String.IsNullOrEmpty(Settings.OutputResultFileName) then
        printfn "Parameter in app.config OutputResultFileName can't be empty."

    if not(Directory.Exists(Settings.OutputDownloadFilesDir)) then
       Directory.CreateDirectory(Settings.OutputDownloadFilesDir) |> ignore
       printfn "OutputDirectory %s created."  Settings.OutputDownloadFilesDir
    if not(Directory.Exists(Settings.ConfigDir)) then
       Directory.CreateDirectory(Settings.ConfigDir) |> ignore
       printfn "Input Source Linkes file - %s not found!"  (Settings.ConfigDir + Settings.LinksDataFileInConfigDir)
    if not(Directory.Exists(Settings.OutputResultDir)) then
        Directory.CreateDirectory(Settings.OutputResultDir) |> ignore
        printfn "OutputDirectory %s created."  Settings.OutputResultDir
       

[<EntryPoint>]
let main _ =

    try
        checkDirStructure()

        printfn "Start download and processing files."
    
        let output_dir = Settings.OutputDownloadFilesDir + Settings.BlockFilePrefix
        let config_dir = Settings.ConfigDir
    
        let stopWatch = System.Diagnostics.Stopwatch.StartNew()
        let trimCharAr = [|'"'|]
            
        let lines = File.ReadAllLines(Path.Combine(config_dir, Settings.LinksDataFileInConfigDir)) 

        lines 
        |> Seq.iter(fun line -> let splitted = line.Split("|")
                                let url = splitted.[0].Trim(trimCharAr)
                                let partfilename = splitted.[1].Trim(trimCharAr)
                                let start_str = splitted.[2].Trim(trimCharAr)
                                let startstr = if not(start_str = "String.Empty") then start_str else String.Empty
                                let repl_str = splitted.[3].Trim(trimCharAr)
                                let replstr = if not(repl_str = "String.Empty") then repl_str else String.Empty

                                download_processing_file(url, output_dir + partfilename, startstr, replstr)                           
        )
    
        File.WriteAllLines(Path.Combine(Settings.OutputResultDir,Settings.OutputResultFileName), dict_for_filter.Keys)
    
        stopWatch.Stop()
        System.Console.WriteLine("Download and Preprocessing Time elapsed: {0}", stopWatch.Elapsed)
    
        dict_for_filter.Clear()
    
    with
    | :? AggregateException as ex -> printfn "Error - %s" ex.Message
    | _ as ex -> printfn "Error - %s" ex.Message

    printfn "Finished."
    0 // return an integer exit code

    (*download_processing_file("https://hosts.ubuntu101.co.za/hosts.windows", output_dir + "ubuntu.txt", "# START HOSTS LIST","127.0.0.1 ", output_path)
    download_processing_file("https://hosts.ubuntu101.co.za/superhosts.deny", output_dir + "super.txt", "# ##### START Super hosts.deny", "ALL: ", output_path)
    download_processing_file("https://raw.githubusercontent.com/FadeMind/hosts.extras/master/add.2o7Net/hosts", output_dir + "2o7.txt", String.Empty, "0.0.0.0 ", output_path)
    download_processing_file("https://raw.githubusercontent.com/hoshsadiq/adblock-nocoin-list/master/hosts.txt", output_dir + "adblocknocoin_host.txt", String.Empty, "0.0.0.0 ", output_path)
    download_processing_file("https://raw.githubusercontent.com/FadeMind/hosts.extras/master/add.Risk/hosts", output_dir + "risk_host.txt", String.Empty, "0.0.0.0 ", output_path)
    download_processing_file("https://raw.githubusercontent.com/FadeMind/hosts.extras/master/add.Spam/hosts", output_dir + "spam_host.txt", String.Empty, "0.0.0.0 ", output_path)
    download_processing_file("https://s3.amazonaws.com/lists.disconnect.me/simple_ad.txt", output_dir + "simple_ad_host.txt", String.Empty, "0.0.0.0 ", output_path)
    download_processing_file("https://raw.githubusercontent.com/Yhonay/antipopads/master/hosts", output_dir + "antipopads_host.txt", String.Empty, "0.0.0.0 ", output_path)
    // anudeepND_blacklist_ad_server
    download_processing_file("https://raw.githubusercontent.com/anudeepND/blacklist/master/adservers.txt", output_dir + "anudeepND_host.txt", String.Empty, "0.0.0.0 ", output_path)
    // Bad_JAV_Sites
    download_processing_file("https://raw.githubusercontent.com/Ultimate-Hosts-Blacklist/Bad_JAV_Sites/master/domains.list", output_dir + "Bad_JAV_host.txt", String.Empty, "0.0.0.0 ", output_path)

    download_processing_file("https://raw.githubusercontent.com/mitchellkrogza/Badd-Boyz-Hosts/master/domains", output_dir + "Badd-Boyz_host.txt", String.Empty, "0.0.0.0 ", output_path)
    download_processing_file("https://raw.githubusercontent.com/Ultimate-Hosts-Blacklist/blacklist/master/domains.list", output_dir + "Blacklist_host.txt", String.Empty, "0.0.0.0 ", output_path)
    download_processing_file("https://raw.githubusercontent.com/Ultimate-Hosts-Blacklist/cameleon_at_sysctl.org/master/domains.list", output_dir + "cameleon_at_sysctl_host.txt", String.Empty, "0.0.0.0 ", output_path)
    download_processing_file("https://raw.githubusercontent.com/bigdargon/hostsVN/master/hosts", output_dir + "hostsVN_host.txt", String.Empty, "0.0.0.0 ", output_path)
    download_processing_file("https://raw.githubusercontent.com/Ultimate-Hosts-Blacklist/justdomains_mirror1.malwaredomains.com/master/domains.list", output_dir + "malwaredomains_host.txt", String.Empty, "0.0.0.0 ", output_path)
    download_processing_file("https://raw.githubusercontent.com/Ultimate-Hosts-Blacklist/KADhosts_azet12/master/domains.list", output_dir + "KADhosts_azet12_host.txt", String.Empty, "0.0.0.0 ", output_path)
    
    download_processing_file("https://raw.githubusercontent.com/Ultimate-Hosts-Blacklist/lightswitch05_hosts_ads-and-tracking-extended/master/domains.list", output_dir + "lightswitch05_hosts_ads_host.txt", String.Empty, "0.0.0.0 ", output_path)
    download_processing_file("https://raw.githubusercontent.com/Ultimate-Hosts-Blacklist/MalwareDomainList.com/master/domains.list", output_dir + "MalwareDomainList_hosts_ads_host.txt", String.Empty, "0.0.0.0 ", output_path)
    download_processing_file("https://raw.githubusercontent.com/Ultimate-Hosts-Blacklist/Michael_Trimms_Hosts/master/domains.list", output_dir + "Michael_Trimms_hosts_ads_host.txt", String.Empty, String.Empty, output_path)
    download_processing_file("https://raw.githubusercontent.com/Ultimate-Hosts-Blacklist/MinimalHostsBlocker/master/domains.list", output_dir + "MinimalHostsBlocker_hosts_ads_host.txt", String.Empty, String.Empty, output_path)
    download_processing_file("https://raw.githubusercontent.com/Ultimate-Hosts-Blacklist/Phishing.Database/master/domains.list", output_dir + "MinimalHostsBlocker_hosts_ads_host.txt", String.Empty, String.Empty, output_path)
    download_processing_file("https://raw.githubusercontent.com/Ultimate-Hosts-Blacklist/pl-host-file/master/domains.list", output_dir + "pl-host_host.txt", String.Empty, String.Empty, output_path)
    download_processing_file("https://raw.githubusercontent.com/Ultimate-Hosts-Blacklist/quidsup_malicious-sites/master/domains.list", output_dir + "quidsup_malicious_host.txt", String.Empty, String.Empty, output_path)
    download_processing_file("https://raw.githubusercontent.com/Ultimate-Hosts-Blacklist/quidsup_notrack_trackers/master/domains.list", output_dir + "quidsup_notrack_trackers_host.txt", String.Empty, String.Empty, output_path)
    download_processing_file("https://raw.githubusercontent.com/Ultimate-Hosts-Blacklist/smed79_blacklist/master/domains.list", output_dir + "smed79_blacklists_host.txt", String.Empty, String.Empty, output_path)
    download_processing_file("https://raw.githubusercontent.com/Ultimate-Hosts-Blacklist/Spam404/master/domains.list", output_dir + "spam404_blacklists_host.txt", String.Empty, String.Empty, output_path)

    download_processing_file("https://raw.githubusercontent.com/Ultimate-Hosts-Blacklist/The-Big-List-of-Hacked-Malware-Web-Sites/master/domains.list", output_dir + "Hacked-Malware_host.txt", String.Empty, String.Empty, output_path)
    download_processing_file("https://raw.githubusercontent.com/Ultimate-Hosts-Blacklist/WaLLy3K_Blacklist/master/domains.list", output_dir + "Hacked-Malware_host.txt", String.Empty, String.Empty, output_path)
    download_processing_file("https://raw.githubusercontent.com/Ultimate-Hosts-Blacklist/YousList/master/domains.list", output_dir + "YousList_host.txt", String.Empty, String.Empty, output_path)
    download_processing_file("https://raw.githubusercontent.com/Ultimate-Hosts-Blacklist/ZeroDot1_CoinBlockerLists/master/domains.list", output_dir + "ZeroDot1_CoinBlocker_host.txt", String.Empty, String.Empty, output_path)

    download_processing_file("https://raw.githubusercontent.com/anudeepND/blacklist/master/adservers.txt", output_dir + "adservers_host.txt", String.Empty, "0.0.0.0 ", output_path)
    download_processing_file("https://raw.githubusercontent.com/anudeepND/blacklist/master/facebook.txt", output_dir + "facebook_host.txt", String.Empty, "0.0.0.0 ", output_path)
    download_processing_file("https://raw.githubusercontent.com/anudeepND/blacklist/master/CoinMiner.txt", output_dir + "coinminer_host.txt", String.Empty, "0.0.0.0 ", output_path)

    download_processing_file("https://raw.githubusercontent.com/StevenBlack/hosts/master/hosts", output_dir + "stevenblack_host.txt", String.Empty, "0.0.0.0 ", output_path)
    download_processing_file("https://raw.githubusercontent.com/StevenBlack/hosts/master/alternates/fakenews/hosts", output_dir + "fakenews_host.txt", String.Empty, "0.0.0.0 ", output_path)
    download_processing_file("https://raw.githubusercontent.com/StevenBlack/hosts/master/alternates/gambling/hosts", output_dir + "gambling_host.txt", String.Empty, "0.0.0.0 ", output_path)
    download_processing_file("https://raw.githubusercontent.com/StevenBlack/hosts/master/alternates/social/hosts", output_dir + "social_host.txt", String.Empty, "0.0.0.0 ", output_path)
    download_processing_file("https://raw.githubusercontent.com/StevenBlack/hosts/master/alternates/fakenews-gambling/hosts", output_dir + "fakenews-gambling_host.txt", String.Empty, "0.0.0.0 ", output_path)

    download_processing_file("https://raw.githubusercontent.com/StevenBlack/hosts/master/data/StevenBlack/hosts", output_dir + "stevenblack_host.txt", String.Empty, "0.0.0.0 ", output_path)
    download_processing_file("https://raw.githubusercontent.com/AdAway/adaway.github.io/master/hosts.txt", output_dir + "adaway_host.txt", String.Empty, "127.0.0.1 ", output_path)
    
    download_processing_file("https://raw.githubusercontent.com/FadeMind/hosts.extras/master/add.2o7Net/hosts", output_dir + "2o7net_host.txt", String.Empty, "0.0.0.0 ", output_path)
    download_processing_file("https://raw.githubusercontent.com/FadeMind/hosts.extras/master/add.Dead/hosts", output_dir + "adddead_host.txt", String.Empty, "0.0.0.0 ", output_path)
    download_processing_file("https://raw.githubusercontent.com/FadeMind/hosts.extras/master/add.Risk/hosts", output_dir + "addrisk_host.txt", String.Empty, "0.0.0.0 ", output_path)

    download_processing_file("https://raw.githubusercontent.com/FadeMind/hosts.extras/master/add.Spam/hosts", output_dir + "addspam_host.txt", String.Empty, "0.0.0.0 ", output_path)
    download_processing_file("https://raw.githubusercontent.com/AdguardTeam/cname-trackers/master/combined_disguised_trackers_justdomains.txt", output_dir + "cdt_host.txt", String.Empty, String.Empty, output_path)
    download_processing_file("https://raw.githubusercontent.com/mitchellkrogza/Badd-Boyz-Hosts/master/hosts", output_dir + "Badd-Boyz_host.txt", String.Empty, "0.0.0.0 ", output_path)
    download_processing_file("https://raw.githubusercontent.com/bigdargon/hostsVN/master/option/hosts-VN", output_dir + "hostsVN_host.txt", String.Empty, "0.0.0.0 ", output_path)

    download_processing_file("https://raw.githubusercontent.com/PolishFiltersTeam/KADhosts/master/KADhosts.txt", output_dir + "KADhosts_host.txt", String.Empty, "0.0.0.0 ", output_path)
    download_processing_file("https://raw.githubusercontent.com/MetaMask/eth-phishing-detect/master/src/hosts.txt", output_dir + "eth-phishing_host.txt", String.Empty, "0.0.0.0 ", output_path)
    download_processing_file("https://raw.githubusercontent.com/jamiemansfield/minecraft-hosts/master/lists/tracking.txt", output_dir + "minecraft-hosts_host.txt", String.Empty, "0.0.0.0 ", output_path)
    
    download_processing_file("https://winhelp2002.mvps.org/hosts.txt", output_dir + "winhelp2002_host.txt", String.Empty, "0.0.0.0 ", output_path)
    download_processing_file("https://raw.githubusercontent.com/davidonzo/Threat-Intel/master/lists/latestdomains.piHole.txt", output_dir + "piHole1_host.txt", String.Empty, "0.0.0.0 ", output_path)
    download_processing_file("https://raw.githubusercontent.com/shreyasminocha/shady-hosts/main/hosts", output_dir + "shady-hosts_host.txt", String.Empty, "0.0.0.0 ", output_path)
    download_processing_file("https://someonewhocares.org/hosts/zero/hosts", output_dir + "someonewhocares_host.txt", String.Empty, "0.0.0.0 ", output_path)

    download_processing_file("https://raw.githubusercontent.com/tiuxo/hosts/master/ads", output_dir + "tiuxo_host.txt", String.Empty, "0.0.0.0 ", output_path)
    download_processing_file("https://raw.githubusercontent.com/FadeMind/hosts.extras/master/UncheckyAds/hosts", output_dir + "UncheckyAds_host.txt", String.Empty, "0.0.0.0 ", output_path)

    download_processing_file("https://urlhaus.abuse.ch/downloads/hostfile/", output_dir + "UncheckyAds_host.txt", String.Empty, "127.0.0.1	", output_path)
    download_processing_file("https://pgl.yoyo.org/adservers/serverlist.php?hostformat=hosts&mimetype=plaintext&useip=0.0.0.0", output_dir + "yoyo_host.txt", String.Empty, "0.0.0.0 ", output_path)

    download_processing_file("https://malwareworld.com/textlists/suspiciousDomains.txt", output_dir + "500_host.txt", String.Empty, String.Empty, output_path)
    download_processing_file("https://data.iana.org/TLD/tlds-alpha-by-domain.txt", output_dir + "BADTLD_host.txt", String.Empty, String.Empty, output_path)

    download_processing_file("https://github.com/T145/black-mirror/releases/download/latest/black_domain.txt", output_dir + "black_domain.txt", String.Empty, String.Empty, output_path)*)
