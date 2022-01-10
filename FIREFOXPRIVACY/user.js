// Disable updates.
user_pref("app.update.auto", false);
user_pref("app.update.enabled", false);
user_pref("app.update.auto.migrated", false);
user_pref("app.update.checkInstallTime", false);
user_pref("app.update.service.enabled", false);
user_pref("app.update.url", "");
user_pref("app.update.url.manual", "");
user_pref("app.update.BITS.enabled", false);
user_pref("extensions.update.autoUpdateDefault", false);
user_pref("extensions.autoupdate.enabled", false);
user_pref("app.update.background.interval", 15000000);
user_pref("extensions.update.enabled", false);
user_pref("extensions.update.background.url", "");
user_pref("extensions.update.url", "");
user_pref("extensions.systemAddon.update.enabled", false);
user_pref("extensions.getAddons.cache.enabled", false);
user_pref("extensions.webextensions.restrictedDomains", "");
user_pref("services.sync.prefs.sync.browser.search.update", false);
user_pref("services.sync.prefs.sync.extensions.update.enabled", false);
user_pref("app.update.background.scheduling.enabled", false);

// Private Browsing VPN
user_pref("browser.privatebrowsing.vpnpromourl", "");

// always ask for download directory
user_pref("browser.download.useDownloadDir", false);
// No need to warn us
user_pref("general.warnOnAboutConfig", false);
user_pref("network.warnOnAboutNetworking", false);
// Do not show about:config warning message
// Не предупреждать при заходе на about:config
user_pref("browser.aboutConfig.showWarning", false);
user_pref("browser.warnOnQuit", false);
user_pref("browser.tabs.warnOnClose", false);
user_pref("browser.tabs.warnOnCloseOtherTabs", false);
user_pref("browser.search.hiddenOneOffs", "Google,Yahoo,Bing,Amazon.com,DuckDuckGo,Twitter,Wikipedia (en)");
user_pref("browser.urlbar.oneOffSearches", false);
user_pref("browser.shell.checkDefaultBrowser", false);
user_pref("browser.shell.didSkipDefaultBrowserCheckOnFirstRun", true);

//Установите одно из следующих значений:
//1 – блокировка всех сторонних куки
//2 – блокировка всех куки
//3 – блокировка куки с непосещенных сайтов
//4 – новая политика Cookie Jar (предотвращение доступа куки к хранилищу)
//5 – динамическая изоляция собственных куки
user_pref("network.cookie.cookieBehavior", 5);
user_pref("browser.privatebrowsing.autostart", true);
user_pref("network.cookie.thirdparty.sessionOnly", true);


// Disable experiments.
user_pref("app.normandy.api_url", "");
user_pref("app.normandy.enabled", false);
user_pref("app.normandy.user_id", "");
user_pref("app.normandy.first_run", false);
user_pref("app.normandy.shieldLearnMoreUrl", "");
user_pref("app.shield.optoutstudies.enabled", false);
//// experiments
user_pref("network.allow-experiments", false);
user_pref("experiments.supported",false);
user_pref("experiments.enabled",false);
user_pref("experiments.manifest.uri", "");
user_pref("experiments.manifest.fetchIntervalSeconds", 86400000); // 86400000 [default is 86400]
user_pref("extensions.ui.experiment.hidden", true);
user_pref("extensions.shield-recipe-client.enabled", false);
user_pref("extensions.shield-recipe-client.api_url", "");
user_pref("camera.control.face_detection.enabled", false);
user_pref("dom.serviceWorkers.enabled",	false);
user_pref("dom.webnotifications.enabled", false);


// Enable HTTPS-Only Mode in all windows
// Включить режим "Только HTTPS" во всех окнах
user_pref("dom.security.https_only_mode", true);
user_pref("dom.security.https_only_mode.upgrade_local", true);
user_pref("dom.security.https_only_mode_ever_enabled", true);
user_pref("dom.security.https_only_mode_ever_enabled_pbm", true);
user_pref("dom.security.https_only_mode_pbm", true);
user_pref("dom.security.https_only_mode_send_http_background_request", true);

user_pref("intl.accept_languages", "en-US,en;q=0.5");
user_pref("intl.locale.matchOS", false);
user_pref("intl.locale.requested", "en-US");
user_pref("browser.search.region", "US");
user_pref("browser.search.countryCode", "US");
user_pref("doh-rollout.home-region", "US");

// Geo Убираем информацию о нашем местоположении.
user_pref("geo.enabled", false);
user_pref("geo.cell.scan", false);
user_pref("geo.wifi.logging.enabled", false);
user_pref("geo.wifi.uri", "");
user_pref("browser.search.geoip.url", "");

user_pref("browser.search.geoip.timeout", 1);//[default is 3000]
user_pref("browser.search.geoSpecificDefaults", false);
user_pref("browser.search.geoSpecificDefaults.url", "");
user_pref("browser.search.suggest.enabled", false);
user_pref("geo.provider.ms-windows-location", false);
user_pref("geo.provider.network.url", "");
user_pref("geo.provider.use_corelocation", false);
user_pref("geo.provider.use_gpsd", false);
user_pref("browser.geolocation.warning.infoURL", "");
user_pref("geo.wifi.scan", false);
user_pref("geo.wifi.timeToWaitBeforeSending", 1576800000);


// Correct Permissions
// permissions 0:Ask / 1:Allow / 2:Don't ask
user_pref("permissions.default.desktop-notification", 2);
user_pref("permissions.default.microphone", 0);
user_pref("permissions.default.camera", 0);
user_pref("permissions.default.geo", 2);
// TOR Uplifts features:
//Send only the scheme, host, and port in the Referer header
//0 = Send the full URL in the Referer header
//1 = Send the URL without its query string in the Referer header
//2 = Send only the scheme, host, and port in the Referer header
user_pref("network.http.referer.trimmingPolicy", 2)
//Only send Referer header when the full hostnames match. (Note: if you notice significant breakage, you might try 1 combined with an XOriginTrimmingPolicy tweak below.) Source
//0 = Send Referer in all cases
//1 = Send Referer to same eTLD sites
//2 = Send Referer only when the full hostnames match
ser_pref("network.http.referer.XOriginPolicy", 2);
//When sending Referer across origins, only send scheme, host, and port in the Referer header of cross-origin requests. Source
//0 = Send full url in Referer
//1 = Send url without query string in Referer
//2 = Only send scheme, host, and port in Referer
ser_pref("network.http.referer.XOriginTrimmingPolicy", 2);


// HTTPS (SSL/TLS / OCSP / CERTS / HPKP)
user_pref("security.tls.version.enable-deprecated", false);
/** CERTS / HPKP (HTTP Public Key Pinning) ***/
// 1 = block all // 0=disable detecting Family Safety mode and importing the root
user_pref("security.family_safety.mode", 0);
// PKP (Public Key Pinning) 0=disabled, 1=allow user MiTM (such as your antivirus), 2=strict default=1
user_pref("security.remote_settings.crlite_filters.enabled", true);
user_pref("security.pki.crlite_mode", 2);
//user_pref("security.csp.experimentalEnabled", true);
//user_pref("security.csp.enable", true);
//user_pref("security.sri.enable", true);
user_pref("security.csp.experimentalEnabled", false);
user_pref("security.csp.enable", false);
//user_pref("security.sri.enable", true);

user_pref("security.OCSP.enabled", 1);
user_pref("security.ssl.enable_ocsp_stapling", true);
user_pref("security.ssl.enable_ocsp_must_staple", true);
//user_pref("security.OCSP.require", true);
user_pref("security.OCSP.require", false);

// PREF: Only allow TLS 1.[2-3]
user_pref("security.tls.version.min", 3);
user_pref("security.tls.version.max", 4);
user_pref("security.tls.version.fallback-limit", 4);
user_pref("security.cert_pinning.enforcement_level", 2);
user_pref("security.pki.sha1_enforcement_level", 1);
user_pref("security.ssl.treat_unsafe_negotiation_as_broken", true);

// Network
user_pref("network.dns.disableIPv6", true);
user_pref("network.http.speculative-parallel-limit", 0);
user_pref("network.cookie.prefsMigrated", true);
user_pref("network.tcp.tcp_fastopen_enable", true);
user_pref("network.proxy.no_proxies_on", "192.168..1/24");
user_pref("network.proxy.socks_remote_dns", true);
user_pref("network.predictor.cleaned-up", true);
user_pref("network.http.pipelining", true);
user_pref("network.http.pipelining.ssl", true);
user_pref("network.http.proxy.pipelining", true);
user_pref("network.http.pipelining.firstrequest", true);
user_pref("network.http.proxy.firstrequest", true);
user_pref("network.http.pipelining.maxrequests", 24);

user_pref("network.prefetch-next", false);
user_pref("network.dns.disablePrefetch", true);
user_pref("network.dns.disablePrefetchFromHTTPS", true);
user_pref("network.predictor.enabled", false);

user_pref("network.dns.blockDotOnion", true);
user_pref("network.negotiate-auth.allow-insecure-ntlm-v1", false);


// Fast Computer Fast Connection
user_pref("content.interrupt.parsing", true);
user_pref("content.max.tokenizing.time", 2250000);
user_pref("content.notify.interval", 750000);
user_pref("content.notify.ontimer", true);
user_pref("content.switch.threshold", 750000);
user_pref("nglayout.initialpaint.delay", 0); // пауза перед началом прорисовки страницы.
user_pref("network.http.max-connections", 42);
user_pref("network.http.max-connections-per-server", 24);
user_pref("network.http.max-persistent-connections-per-proxy", 16);
user_pref("network.http.max-persistent-connections-per-server", 8);
user_pref("network.security.ports.banned", "9050,9051,9150,9151");

// Device Drive
//ssd optim
user_pref("browser.sessionstore.interval", 15000000);
//ssd optim [prevents lazy loading] [performance enhancement]
user_pref("browser.sessionstore.restore_on_demand", false);
//ssd optim [prevents frequent writing to drive]
user_pref("browser.sessionstore.resume_from_crash", false);
user_pref("browser.cache.memory.enable", true);
user_pref("browser.cache.memory.capacity", 1048576); //1GB
user_pref("browser.cache.memory.max_entry_size", 1048576); //1GB
user_pref("browser.cache.disk.enable", false);
//user_pref("browser.cache.disk.capacity", 153600);
//user_pref("browser.cache.disk.amount_written", 71050);
user_pref("browser.cache.offline.storage.enable", false);
user_pref("browser.cache.offline.enable", false);
user_pref("browser.cache.disk.smart_size.enabled", false);
user_pref("browser.cache.disk_cache_ssl", false);
// [stops per site cross-site image requests]
user_pref("browser.cache.use_new_backend_temp", false);
user_pref("browser.cache.offline.capacity", 0);
user_pref("browser.cache.disk.capacity", 0);
user_pref("browser.privatebrowsing.forceMediaMemoryCache", true);

// Enable DNS-over-HTTPS (DoH)
// 0 - Default value in standard Firefox installations (currently is 5, which means DoH is disabled)
// 1 - DoH is enabled, but Firefox picks if it uses DoH or regular DNS based on which returns faster query responses
// 2 - DoH is enabled, and regular DNS works as a backup
// First. Use TRR first, and only if the name resolve fails use the native resolver as a fallback.
// 3 - DoH is enabled, and regular DNS is disabled.
// Only. Only use TRR. Never use the native (after the initial setup).
// 4 - Shadow. (removed) DON'T SET.
// 5 - DoH is disabled. Off by choice This is the same as 0 but marks it as done by choice and not done by default.
user_pref("network.trr.mode", 5);

//user_pref("network.trr.uri", "https://mozilla.cloudflare-dns.com/dns-query");
//user_pref("network.trr.custom_uri", "https://mozilla.cloudflare-dns.com/dns-query");

//user_pref("network.trr.uri", "https://security.cloudflare-dns.com/dns-query");
//user_pref("network.trr.custom_uri", "https://security.cloudflare-dns.com/dns-query");
//user_pref("network.trr.bootstrapAddress", "1.1.1.2");
//user_pref("network.trr.confirmationNS", "cloudflare-dns.com");
//user_pref("network.trr.credentials", "cloudflare-dns.com");

//user_pref("network.trr.uri", "https://doh.eu.dnswarden.com/adblock");
//user_pref("network.trr.bootstrapAddress", "45.76.88.20"); // 38 ms
//user_pref("network.trr.confirmationNS", "doh.eu.dnswarden.com");
//user_pref("network.trr.credentials", "doh.eu.dnswarden.com");
//user_pref("network.trr.custom_uri", "https://doh.eu.dnswarden.com/adblock");

//user_pref("network.trr.uri", "https://doh.nl.ahadns.net/dns-query");
//user_pref("network.trr.bootstrapAddress", "5.2.75.75");
//user_pref("network.trr.confirmationNS", "doh.nl.ahadns.net");
//user_pref("network.trr.credentials", "doh.nl.ahadns.net");
//user_pref("network.trr.custom_uri", "https://doh.nl.ahadns.net/dns-query");
//user_pref("doh-rollout.doneFirstRun", false);

user_pref("network.trr.uri", "");
user_pref("network.trr.bootstrapAddress", "");
user_pref("network.trr.confirmationNS", "");
user_pref("network.trr.credentials", "");
user_pref("network.trr.custom_uri", "https://odoh.cloudflare-dns.com/dns-query");
user_pref("doh-rollout.doneFirstRun", false);

user_pref("network.trr.skip-check-for-blocked-host", false);
user_pref("network.url.useDefaultURI", false);
user_pref("network.trr.default_provider_uri", "");
user_pref("network.trr.confirmation_telemetry_enabled", false);
user_pref("network.trr.disable-ECS", true);
user_pref("network.trr.early-AAAA", true);
user_pref("network.trr.wait-for-A-and-AAAA", false);
user_pref("network.trr.wait-for-portal", true);
user_pref("network.trr.useGET", false);
user_pref("network.trr.enable_when_nrpt_detected", true);
user_pref("network.trr.enable_when_proxy_detected", true);
user_pref("network.trr.enable_when_vpn_detected", true);
user_pref("network.dns.skipTRR-when-parental-control-enabled", false);
user_pref("network.trr.blocklist_cleanup_done", true);
// Enable Encrypted Client Hello (ECH) ECH replaces ESNI in Firefox 85
// Включить Encrypted Client Hello (ECH)
user_pref("network.dns.echconfig.enabled", true);
user_pref("network.dns.use_https_rr_as_altsvc", true);


// Oblivious DNS Over HTTPS (ODoH - anonymized dns query) better than DNS-over-HTTPS (DoH)
user_pref("network.trr.mode", 3);
user_pref("network.trr.odoh.enabled", true);
user_pref("network.trr.odoh.min_ttl", 60);
//user_pref("network.trr.odoh.target_host", "https://odoh-target-rs.crypto-team.workers.dev");
//user_pref("network.trr.odoh.target_path", "");
//user_pref("network.trr.odoh.proxy_uri", "https://alpha-odoh-rs-proxy.research.cloudflare.com");
//user_pref("network.trr.odoh.configs_uri", "");
user_pref("network.trr.odoh.proxy_uri", "https://odoh1.surfdomeinen.nl/proxy");
user_pref("network.trr.odoh.target_host", "https://odoh.cloudflare-dns.com");
user_pref("network.trr.odoh.configs_uri", "https://odoh.cloudflare-dns.com/.well-known/odohconfigs");
user_pref("network.trr.odoh.target_path", "dns-query");


user_pref("network.http.spdy.enabled", true);
user_pref("network.http.spdy.enabled.http2", true);

user_pref("network.http.http3.enabled", true);
user_pref("network.http.http3.enable_qlog", true);

//WebRTC Leak Prevent. Turn off WebRTC. Отключение пирингового клиента и WebRTC
user_pref("media.navigator.enabled", false);
user_pref("media.peerconnection.enabled", false);
user_pref("media.navigator.video.enabled", false);
// Firefox >= 52
user_pref("media.peerconnection.ice.no_host", true);
user_pref("media.peerconnection.ice.default_address_only", true);
user_pref("media.peerconnection.ice.proxy_only_if_behind_proxy", true);
user_pref("media.peerconnection.ice.stun_client_maximum_transmits", 0);
user_pref("media.peerconnection.ice.relay_only", true);
user_pref("media.peerconnection.identity.enabled", false);
user_pref("media.peerconnection.identity.timeout", 1);
user_pref("media.peerconnection.turn.disable", true);
user_pref("media.peerconnection.simulcast", false);
user_pref("media.peerconnection.default_iceservers", []);
user_pref("media.getusermedia.screensharing.enabled", false);
user_pref("media.getusermedia.audiocapture.enabled", false);
user_pref("media.peerconnection.use_document_iceservers", false);
user_pref("media.peerconnection.video.enabled", false);
user_pref("media.peerconnection.video.h264_enabled", false);
user_pref("media.av1.enabled", true);
//Отключаем распознавание и синтез речи
user_pref("media.webspeech.recognition.enable", false);
user_pref("media.webspeech.synth.enabled", false);

// Turn on media control
// Включить элементы управления мультимедиа
user_pref("media.hardwaremediakeys.enabled", true);

// Отвечает за отправку статистики о воспроизведении видео (кол-во пропущенных/отрендеренных кадров, и т.д.)
user_pref("media.video_stats.enabled", false);
user_pref("media.gmp-gmpopenh264.enabled", false);
user_pref("media.gmp-gmpopenh264.autoupdate", false);
user_pref("media.gmp-manager.url", "");
user_pref("media.gmp-manager.url.override", "");
user_pref("media.gmp-manager.updateEnabled", false);
user_pref("media.gmp.trial-create.enabled", false);

// WebGL and DRM media
user_pref("webgl.disabled", true);
user_pref("media.eme.enabled", false);
user_pref("browser.eme.ui.enabled", false);
user_pref("media.autoplay.blocking_policy", 2);
//user_pref("media.benchmark.vp9.fps", 215);
//user_pref("media.benchmark.vp9.versioncheck", 5);
user_pref("media.gmp-widevinecdm.enabled", false);
user_pref("media.gmp-widevinecdm.visible", false);
user_pref("media.gmp.storage.version.observed", 1);


user_pref("media.fragmented-mp4.exposed", true);
user_pref("media.fragmented-mp4.ffmpeg.enabled", true);
user_pref("media.mediasource.mp4.enabled", true);
user_pref("media.mediasource.youtubeonly", false);
user_pref("media.videocontrols.picture-in-picture.video-toggle.enabled", false);
user_pref("media.decoder-doctor.notifications-allowed", false);

user_pref("media.autoplay.default", 1);
user_pref("media.autoplay.allow-muted", false);
user_pref("media.mediasource.enabled", true);

// Disable screensharing framework
// [enabling allows video confrencing/screen chat]
user_pref("media.getusermedia.screensharing.allowed_domains", "");
// [default is 20190417124012 ]
//user_pref("media.gmp-manager.buildID", 0);
//user_pref("media.gmp-provider.enabled", false);
//[DRM related and prevents loading of Google's widevine]
// [DRM related]*/
// [doubled default size of 524288] [performance enhancement]
user_pref("media.memory_caches_combined_limit_kb", 1048576); //1GB
user_pref("media.memory_cache_max_size", 2097152); //2GB
// [quadrupled default size of 8192] [performance enhancement]

//Забываем про Adobe Primetime Content Decryption Module (DRM)
user_pref("media.eme.apiVisible", false);
user_pref("media.gmp-eme-adobe.enabled", false);
user_pref("media.getusermedia.aec_enabled", false);
user_pref("media.getusermedia.agc_enabled", false);
user_pref("media.getusermedia.browser.enabled", false);
user_pref("media.getusermedia.noise_enabled", false);
user_pref("media.getusermedia.screensharing.allow_on_old_platforms", false);
user_pref("media.navigator.permission.disabled", false);
user_pref("media.ondevicechange.enabled", true);


// SafeBrowsing Enable
user_pref("browser.safebrowsing.allowOverride", true); // Enables/disables the "Ignore this warning" button on pages the SafeBrowsing blocks. 
user_pref("browser.safebrowsing.enabled", true);
user_pref("services.sync.prefs.sync.browser.safebrowsing.enabled", true);
user_pref("browser.safebrowsing.allowOverride", false);

// Disable retrieval of safebrowsing lists
user_pref("services.sync.prefs.sync.browser.safebrowsing.malware.enabled", true);
user_pref("services.sync.prefs.sync.browser.safebrowsing.phishing.enabled", true);
user_pref("browser.safebrowsing.blockedURIs.enabled", true);
user_pref("browser.safebrowsing.phishing.enabled", true);
user_pref("browser.safebrowsing.malware.enabled", true);
user_pref("browser.safebrowsing.downloads.enabled", true);
user_pref("browser.safebrowsing.downloads.remote.enabled", true);
user_pref("browser.safebrowsing.downloads.remote.block_dangerous", true);
user_pref("browser.safebrowsing.downloads.remote.block_dangerous_host", true);
user_pref("browser.safebrowsing.downloads.remote.block_potentially_unwanted", true);
user_pref("browser.safebrowsing.downloads.remote.block_uncommon", true);

// Отключаем отправку информации о всех посещаемых сайтах и закаченных файлов, на ресурсы google и mozilla, для проверки на "вредоносность".
user_pref("browser.safebrowsing.gethashURL", "");
user_pref("browser.safebrowsing.provider.google.appRepURL", "");
user_pref("browser.safebrowsing.reportErrorURL", "");
user_pref("browser.safebrowsing.reportGenericURL", "");
user_pref("browser.safebrowsing.reportMalwareErrorURL", "");
user_pref("browser.safebrowsing.reportMalwareMistakeURL", "");
user_pref("browser.safebrowsing.reportMalwareURL", "");
user_pref("browser.safebrowsing.reportPhishMistakeURL", "");

//что-то-там про приватность, отправку в гугл и т.д.
user_pref("browser.safebrowsing.appRepURL", "");
user_pref("browser.safebrowsing.malware.reportURL", "");
user_pref("browser.safebrowsing.reportURL", "");

user_pref("browser.safebrowsing.provider.google.advisoryURL", "");
user_pref("browser.safebrowsing.provider.google.gethashURL", "");
user_pref("browser.safebrowsing.provider.google.reportMalwareMistakeURL", "");
user_pref("browser.safebrowsing.provider.google.reportPhishMistakeURL", "");
user_pref("browser.safebrowsing.provider.google.reportURL", "");
user_pref("browser.safebrowsing.provider.google.updateURL", "");
user_pref("browser.safebrowsing.provider.google4.advisoryName", "");
user_pref("browser.safebrowsing.provider.google4.advisoryURL", "");
user_pref("browser.safebrowsing.provider.google4.dataSharingURL", "");
user_pref("browser.safebrowsing.provider.google4.gethashURL", "");
user_pref("browser.safebrowsing.provider.google4.reportMalwareMistakeURL", "");
user_pref("browser.safebrowsing.provider.google4.reportPhishMistakeURL", "");
user_pref("browser.safebrowsing.provider.google4.reportURL", "");
user_pref("browser.safebrowsing.provider.mozilla.gethashURL", "");
user_pref("browser.safebrowsing.reportPhishURL", "");

//Разрешить или нет Firefox блокировать расширения и плагины из "чёрного списка", который составляется разработчиками браузера.
// Tracking Protection
user_pref("privacy.trackingprotection.enabled",	true);
user_pref("privacy.trackingprotection.pbmode.enabled", true);
user_pref("privacy.trackingprotection.socialtracking.enabled", true);
user_pref("privacy.trackingprotection.cryptomining.enabled", true);
user_pref("services.sync.prefs.sync.privacy.trackingprotection.cryptomining.enabled", true);
user_pref("privacy.userContext.enabled", true);

// Cache tracking
// Isolate cookies to 1st party domain. This WILL break logins/sessions
// Degrades performance of animations, like scrolling
user_pref("privacy.resistFingerprinting", true);
user_pref("privacy.resistFingerprinting.block_mozAddonManager", true);
user_pref("privacy.donottrackheader.enabled", true);
user_pref("privacy.sanitize.sanitizeOnShutdown", true);
user_pref("privacy.clearOnShutdown.cache", true);
user_pref("privacy.clearOnShutdown.cookies", true);
user_pref("privacy.clearOnShutdown.downloads", true);
user_pref("privacy.clearOnShutdown.formdata", true);
user_pref("privacy.clearOnShutdown.history", true);
user_pref("privacy.clearOnShutdown.offlineApps", true);
user_pref("privacy.clearOnShutdown.sessions", true);
user_pref("privacy.clearOnShutdown.openWindows", true);
user_pref("privacy.sanitize.timeSpan", 0);
user_pref("privacy.cpd.offlineApps", true);
user_pref("privacy.cpd.cache", true);
user_pref("privacy.cpd.cookies", true);
user_pref("privacy.cpd.downloads", true);
user_pref("privacy.cpd.formdata", true);
user_pref("privacy.cpd.history", true);
user_pref("privacy.cpd.sessions", true);
user_pref("privacy.firstparty.isolate", true);

// Disable Onboarding
user_pref("privacy.trackingprotection.ui.enabled", true);
// disable intro
user_pref("privacy.trackingprotection.introCount", 99);
user_pref("privacy.trackingprotection.introURL", "");
// ---------------------------------------------------------
user_pref("network.connectivity-service.IPv4.url", "");
user_pref("network.connectivity-service.IPv6.url", "");

// Disable Firefox Screenshots
user_pref("extensions.screenshots.disabled", true);
user_pref("extensions.screenshots.system-disabled", true);
user_pref("extensions.screenshots.upload-disabled", true);

// Stops leave-page warning
user_pref("dom.disable_beforeunload", true);
//позволяет определять параметры соединения пользователя с сетью.
user_pref("dom.network.enabled", false);

//отвечает за передачу информации серверу о времени начала и конца загрузки страницы. Анализ этих данных может позволить определить факт наличия прокси.
user_pref("dom.enable_performance", false);
user_pref("dom.enable_resource_timing", false);
user_pref("dom.enable_user_timing", false);
user_pref("dom.webaudio.enabled", false);
user_pref("dom.mozTCPSocket.enabled", false);
user_pref("dom.netinfo.enabled", false);
user_pref("dom.telephony.enabled", false);


// PREF: Disallow connection to servers not supporting safe renegotiation (disabled)
//user_pref("security.ssl.require_safe_negotiation", true);
user_pref("security.ssl.errorReporting.automatic", false);
user_pref("browser.ssl_override_behavior", 1);
// PREF: Disable null ciphers
user_pref("security.ssl3.rsa_null_sha",	false);
user_pref("security.ssl3.rsa_null_md5",	false);
user_pref("security.ssl3.ecdhe_rsa_null_sha", false);
user_pref("security.ssl3.ecdhe_ecdsa_null_sha", false);
user_pref("security.ssl3.ecdh_rsa_null_sha", false);
user_pref("security.ssl3.ecdh_ecdsa_null_sha", false);
user_pref("security.ssl3.rsa_seed_sha",	false);
user_pref("security.ssl3.rsa_rc4_40_md5", false);
user_pref("security.ssl3.rsa_rc2_40_md5", false);
user_pref("security.ssl3.rsa_1024_rc4_56_sha", false);
// 128-bit ciphers
user_pref("security.ssl3.rsa_camellia_128_sha",	false);
user_pref("security.ssl3.ecdhe_rsa_aes_128_sha", false);
user_pref("security.ssl3.ecdhe_ecdsa_aes_128_sha", false);
user_pref("security.ssl3.ecdh_rsa_aes_128_sha",	false);
user_pref("security.ssl3.ecdh_ecdsa_aes_128_sha", false);
user_pref("security.ssl3.dhe_rsa_camellia_128_sha", false);
user_pref("security.ssl3.dhe_rsa_aes_128_sha", false);
user_pref("security.ssl3.ecdh_ecdsa_rc4_128_sha", false);
user_pref("security.ssl3.ecdh_rsa_rc4_128_sha",	false);
user_pref("security.ssl3.ecdhe_ecdsa_rc4_128_sha", false);
user_pref("security.ssl3.ecdhe_rsa_rc4_128_sha", false);
user_pref("security.ssl3.rsa_rc4_128_md5", false);
user_pref("security.ssl3.rsa_rc4_128_sha", false);
user_pref("security.tls.unrestricted_rc4_fallback", false);
user_pref("security.ssl3.dhe_dss_des_ede3_sha",	false);
user_pref("security.ssl3.dhe_rsa_des_ede3_sha",	false);
user_pref("security.ssl3.ecdh_ecdsa_des_ede3_sha", false);
user_pref("security.ssl3.ecdh_rsa_des_ede3_sha", false);
user_pref("security.ssl3.ecdhe_ecdsa_des_ede3_sha", false);
user_pref("security.ssl3.ecdhe_rsa_des_ede3_sha", false);
user_pref("security.ssl3.rsa_des_ede3_sha", false);
user_pref("security.ssl3.rsa_fips_des_ede3_sha", false);
user_pref("security.ssl3.ecdh_rsa_aes_256_sha",	false);
user_pref("security.ssl3.ecdh_ecdsa_aes_256_sha", false);
user_pref("security.ssl3.rsa_camellia_256_sha",	false);
// 0xc02b
user_pref("security.ssl3.ecdhe_ecdsa_aes_128_gcm_sha256", true);
// 0xc02f
user_pref("security.ssl3.ecdhe_rsa_aes_128_gcm_sha256",	true);
user_pref("security.ssl3.ecdhe_ecdsa_chacha20_poly1305_sha256",	true);
user_pref("security.ssl3.ecdhe_rsa_chacha20_poly1305_sha256", true);
user_pref("security.ssl3.dhe_rsa_camellia_256_sha", false);
user_pref("security.ssl3.dhe_rsa_aes_256_sha", false);
user_pref("security.ssl3.dhe_dss_aes_128_sha", false);
user_pref("security.ssl3.dhe_dss_aes_256_sha", false);
user_pref("security.ssl3.dhe_dss_camellia_128_sha", false);
user_pref("security.ssl3.dhe_dss_camellia_256_sha", false);

// false - отключить отправку DNS в обход настроек прокси. dns-запросы тоже будут отправляться через туннель
user_pref("extensions.blocklist.enabled", true);

user_pref("social.enabled", false);
user_pref("dom.allow_scripts_to_close_windows", true);
user_pref("dom.disable_window_flip", true);
user_pref("dom.disable_move_resize", true);
user_pref("dom.disable_window_move_resize", true);
user_pref("dom.disable_window_open_feature.close", true);
user_pref("dom.disable_window_open_feature.directories", true);
user_pref("dom.disable_window_open_feature.location", true);
user_pref("dom.disable_window_open_feature.personalbar", true);
user_pref("dom.disable_window_open_feature.resizable", true);
user_pref("dom.disable_window_open_feature.scrollbars", true);
user_pref("dom.disable_window_open_feature.status", true);
user_pref("dom.disable_window_open_feature.titlebar", true);
user_pref("dom.disable_window_status_change", true);
user_pref("dom.event.contextmenu.enabled", false);
user_pref("dom.disable_location.hostname.set", true);
user_pref("browser.download.manager.scanWhenDone", true);
user_pref("dom.storage.enabled", false);


user_pref("dom.indexedDB.enabled", false);

user_pref("browser.newtabpage.activity-stream.telemetry", false);
user_pref("browser.newtabpage.activity-stream.telemetry.ping.endpoint", "");

// Do not allow Firefox to make prezonalized extension recommendations
// Не разрешать Firefox давать персональные рекомендации расширений
// Do not auto-hide Downloads button in toolbar
// Не скрывать кнопку "Загрузки" на панели инструментов
user_pref("browser.download.autohideButton", false);
// Do not automatically open downloads when complete
// Не открывать автоматически скачанный файл по завершению скачивания
user_pref("browser.download.improvements_to_download_panel", false);
// Turn off counting URIs in private browsing mode
// Отключить подсчета URI в приватном режиме просмотра
user_pref("browser.engagement.total_uri_count.pbm", false);
// Turn off Library Highlights
// Скрыть "Последнее Избранное" в Библиотеки
user_pref("browser.library.activity-stream.enabled", false);
// Do not notify about new features
// Не уведомлять о новых функциях Firefox
// Add "View Image Info" to the image context menu
// Добавить в контекстное меню изображений пункт "Информация об изображении"
user_pref("browser.menu.showViewImageInfo", true);
// Turn off recommended extensions
// Отключить рекомендации расширений
// Do not recommend extensions as you browse
// Не рекомендовать расширения при просмотре
user_pref("browser.newtabpage.activity-stream.asrouter.userprefs.cfr.addons", false);
// Do not recommend features as you browse
// Не рекомендовать функции при просмотре
user_pref("browser.newtabpage.activity-stream.asrouter.userprefs.cfr.features", false);
// Turn off Snippets (Updates from Mozilla and Firefox)
// Отключить Заметки (Обновления от Mozilla и Firefox)
// Does not offer import bookmarks, history and passwords from other browsers
// Не предлагать импорт закладок, истории и паролей из другого браузера
user_pref("browser.newtabpage.activity-stream.migrationExpired", true);
// Show Highlights in 4 rows
// Отобразить избранные сайты в 4 столбца
user_pref("browser.newtabpage.activity-stream.section.highlights.rows", 4);
// Hide sponsored top sites in Firefox Home screen
// Скрыть топ сайтов спонсоров на домашней странице Firefox
// Show Top Sites in 4 rows
// Отобразить топ сайтов в 4 столбца
user_pref("browser.newtabpage.activity-stream.topSitesRows", 4);
// Turn on "Firefox Experiments" settings page
// Включить раздел "Эксперименты Firefox" в настройках
user_pref("browser.preferences.experimental", true);
// Show search suggestions in Private Windows
// Отображать поисковые предложения в Приватных вкладках
user_pref("browser.search.suggest.enabled.private", true);
// Set number of saved closed tabs on 20
// Установить количество закрытых вкладок для восстановления на 20
user_pref("browser.sessionstore.max_tabs_undo", 20);
// Restore previous session
// Восстанавливать предыдущую сессию
user_pref("browser.startup.page", 3);
// The last tab does not close the browser
// Не закрывать браузер при закрытии последней вкладки
user_pref("browser.tabs.closeWindowWithLastTab", false);
// Show Title Bar
// Отобразить заголовок
user_pref("browser.tabs.drawInTitlebar", false);
// Show Drag Space
// Отобразить место для перетаскивания окна
user_pref("browser.tabs.extraDragSpace", true);
// Open new tabs on the right
// Открывать новые вкладки справа
user_pref("browser.tabs.insertRelatedAfterCurrent", false);
// Open bookmarks in a background tab
// Открывать закладки в фоновых вкладках
user_pref("browser.tabs.loadBookmarksInBackground", true);
// Show tab previews in the Windows taskbar
// Отображать эскизы вкладок на панели задач
user_pref("browser.taskbar.previews.enable", true);
// Decode copied URLs, containing UTF8 symbols
// Декодировать URL, содержащий UTF8-символы
user_pref("browser.urlbar.decodeURLsOnCopy", true);
// Do not send search term via ISP's DNS server
// Не отправлять поисковый запрос через DNS-сервер провайдера
user_pref("browser.urlbar.dnsResolveSingleWordsAfterSearch", 0);
// When using the address bar, do not suggest search engines
// При использовании панели адреса не предлагать ссылки из поисковых
user_pref("browser.urlbar.suggest.engines", false);
// Alway show bookmarks toolbar
// Всегда отображать панель закладок
// Turn off "Firefox Default Browser Agent"
// Отключить "Firefox Default Browser Agent"
user_pref("default-browser-agent.enabled", false);
// Turn off protection for downloading files over insecure connections
// Отключить защиту скачивания файлов через незащищенные соединения
user_pref("dom.block_download_insecure", true);
// Turn on lazy loading for images
// Включить отложенную загрузку для изображений
// user_pref("dom.dom.image-lazy-loading.enabled", true);
user_pref("dom.dom.image-lazy-loading.enabled", false);
// Run extensions in Private browsing mode
// Запускать дополнения в приватном режиме
user_pref("extensions.allowPrivateBrowsingByDefault", true);
// Turn off Extension Recommendations on the Add-ons Manager
// Отключить рекомендуемые расширения на странице "Дополнения"
// Turn off Pocket
// Отключить Pocket
// Highlight all occurrences of the phrase when searching
// Подстветить всех вхождения фразы в текст при поиске
user_pref("findbar.highlightAll", true);
// Enable site isolation (Project Fission)
// Включить режим строгой изоляции страниц (Project Fission)
user_pref("fission.autostart", true);
// Use smooth scrolling
// Использовать плавную прокрутку
user_pref("general.autoScroll", false);
// Do not select when double-clicking text the space following the text
// Не выделять при выделении слова двойным нажатием идущий за ним пробел
user_pref("layout.word_select.eat_space_to_next_word", false);
// Turn on MMB for openning link a new tab
// Включить открытие ссылки в новой вкладки по нажатию на СКМ
user_pref("middlemouse.openNewWindow", true);
// Set automatic proxy configuration URL
// Block new requests asking to allow notifications
// Блокировать новые запросы на отправку уведомлений
// Send websites a "Do Not Track" signal always
// Передавать сайтам сигнал "Не отслеживать" всегда
// Set time range to clear to "Everything"
// Выбрать "Удалить всё" при удаление истории
// Prompts should be window modal by default
// Привязывать модальные диалоги по умолчанию к окну
user_pref("prompts.defaultModalType", 3);
// Turn on UI customizations sync
// Включить синхронизацию кастомизации интерфейса
// user_pref("services.sync.prefs.sync.browser.uiCustomization.state", true);
user_pref("services.sync.prefs.sync.browser.uiCustomization.state", false);
// Enable the import of passwords as a CSV file on the about:logins page
// Включить импорт паролей в виде CSV-файла на странице "about: logins"
user_pref("signon.management.page.fileImport.enabled", true);
// Enable urlbar built-in calculator
// Включить встроенный в адресную строку калькулятор
user_pref("suggest.calculator", true);
// Show indicators on saved logins that are re-using those breached passwords
// Показать индикаторы на сохраненных логинах, которые повторно используют эти скомпрометированные пароли
user_pref("signon.management.page.vulnerable-passwords.enabled", true);
// Unpin Top Sites search shortcuts
// Открепить ярлыки поисковых сервисов в Топе сайтов
user_pref("browser.newtabpage.activity-stream.improvesearch.topSiteSearchShortcuts", false);
//Масштаб 100% вне зависимости от изменённого масштаба системы
// user_pref("layout.css.devPixelsPerPx", "1.0");

user_pref("general.useragent.locale", "en-US"); // [default is en-US]
user_pref("general.useragent.site_specific_overrides", false);
user_pref("gfx.downloadable_fonts.disable_cache", true);
user_pref("identity.fxaccounts.remote.profile.uri", "");
user_pref("identity.mobilepromo.android", "");
user_pref("identity.mobile.promo", "");
user_pref("identity.mobilepromo.ios", "");
user_pref("layers.shared-buffer-provider.enabled", false);
// [default is 1. setting to 2 additionally forces spell check in search boxes]
user_pref("layout.spellcheckDefault", 0);
user_pref("plugins.update.url", ""); // [stops "install Firefox" weblink nag]
user_pref("security.sandbox.logging.enabled", false);
// [default is 43200]
user_pref("services.sync.telemetry.submissionInterval", 999999999);
user_pref("startup.homepage_override_url", "");
user_pref("startup.homepage_welcome_url", "");
user_pref("content.notify.backoffcount", 5);
user_pref("plugin.expose_full_path", true);
user_pref("ui.submenuDelay", 0);

// Block additional
user_pref("apz.keyboard.enabled", "");
user_pref("browser.chrome.errorReporter.infoURL", "");
user_pref("browser.newtabpage.activity-stream.telemetry.structuredIngestion.endpoint", "");

user_pref("datareporting.healthreport.infoURL", "");
user_pref("datareporting.policy.firstRunURL, "");
user_pref("datareporting.healthreport.uploadEnabled", false);
user_pref("datareporting.policy.dataSubmissionEnabled",	false);
user_pref("datareporting.healthreport.service.enabled", false);
user_pref("datareporting.healthreport.service.firstRun", false);
user_pref("datareporting.sessions.current.clean", true);
user_pref("datareporting.policy.dataSubmissionEnabled.v2", false);
//no data upload
//allow health report upload w/ no prompt
user_pref("datareporting.policy.dataSubmissionPolicyBypassAcceptance", true);

//services.settings.server
user_pref("browser.contentblocking.report.cookie.url", "");
user_pref("browser.contentblocking.report.cryptominer.url", "");
user_pref("browser.contentblocking.report.endpoint_url", "");
user_pref("browser.contentblocking.report.fingerprinter.url", "");
user_pref("browser.contentblocking.report.lockwise.how_it_works.url", "");
user_pref("browser.contentblocking.report.manage_devices.url", "");
user_pref("browser.contentblocking.report.mobile-android.url", "");
user_pref("browser.contentblocking.report.mobile-ios.url", "");
user_pref("browser.contentblocking.report.monitor.enabled", false);
user_pref("browser.contentblocking.report.monitor.home_page_url", "");
user_pref("browser.contentblocking.report.monitor.how_it_works.url", "");
user_pref("browser.contentblocking.report.monitor.preferences_url", "");
user_pref("browser.contentblocking.report.monitor.sign_in_url", "");
user_pref("browser.contentblocking.report.monitor.url", "");
user_pref("browser.contentblocking.report.proxy.enabled", false);
user_pref("browser.contentblocking.report.proxy_extension.url", "");
user_pref("browser.contentblocking.report.show_mobile_app", true);
user_pref("browser.contentblocking.report.social.url", "");
user_pref("browser.contentblocking.report.tracker.url", "");
user_pref("browser.contentblocking.report.vpn-android.url", "");
user_pref("browser.contentblocking.report.vpn-ios.url", "");
user_pref("browser.contentblocking.report.vpn-promo.url", "");
user_pref("browser.contentblocking.report.vpn.enabled", false);
user_pref("browser.contentblocking.report.vpn.url", "");
user_pref("browser.contentblocking.reportBreakage.url, "");

// Инструменты - Настройки - Синхронизация
// Всё что с этим связано - отключаем, убираем, очищаем.
user_pref("identity.fxaccounts.auth.uri", "");
user_pref("identity.fxaccounts.remote.force_auth.uri", "");
user_pref("identity.fxaccounts.remote.signin.uri", "");
user_pref("identity.fxaccounts.remote.signup.uri", "");
user_pref("identity.fxaccounts.settings.uri", "");
user_pref("services.push.serverURL", "");
user_pref("services.sync.engine.addons", false);
user_pref("services.sync.engine.bookmarks", false);
user_pref("services.sync.engine.history", false);
user_pref("services.sync.engine.passwords", false);
user_pref("services.sync.engine.prefs", false);
user_pref("services.sync.engine.tabs", false);
user_pref("services.sync.fxa.privacyURL", "");
user_pref("services.sync.fxa.termsURL", "");
user_pref("services.sync.jpake.serverURL", "");
user_pref("services.sync.privacyURL", "");
user_pref("services.sync.serverURL", "");
user_pref("services.sync.statusURL", "");
user_pref("services.sync.syncKeyHelpURL", "");
user_pref("services.sync.termsURL", "");
user_pref("services.sync.tokenServerURI", "");

//Отключаем телеметрию и экспереминтальные функции.
//(Это метрики о производительности, настройках, оборудовании, для выявления новых проблем и получении мозиллой максимально конкретной информации о ней)
user_pref("experiments.activeExperiment", false);
user_pref("experiments.logging.dump", false);
//user_pref("experiments.manifest.fetchIntervalSeconds", 0);
user_pref("toolkit.identity.enabled", false);
user_pref("toolkit.telemetry.cachedClientID", "");
user_pref("toolkit.telemetry.infoURL", "");
user_pref("toolkit.telemetry.optoutSample", false);
user_pref("toolkit.telemetry.rejected", false);
user_pref("toolkit.telemetry.unifiedIsOptIn", true);

//Отключаем отправку отчетов
user_pref("browser.selfsupport.url", "");
user_pref("browser.tabs.crashReporting.email", "");
user_pref("browser.tabs.crashReporting.emailMe", false);
user_pref("browser.tabs.crashReporting.includeURL", false);
user_pref("datareporting.healthreport.about.reportUrl", "");
user_pref("datareporting.healthreport.about.reportUrlUnified", "");
user_pref("datareporting.healthreport.documentServerURI", "");
user_pref("datareporting.healthreport.logging.consoleEnabled", "");
user_pref("datareporting.healthreport.logging.dumpEnabled", false);

//Отключаем Firefox Share (взаимодействие с соц.сетями)
user_pref("social.share.activationPanelEnabled", false);
user_pref("social.shareDirectory", "");

//Не отправлять данные, для показа вам сниппетов, при посещении домашней страницы.
user_pref("browser.snippets.countryCode", "US");
user_pref("browser.snippets.enabled=", false);
user_pref("browser.snippets.geoUrl", "");
user_pref("browser.snippets.statsUrl", "");
user_pref("browser.snippets.syncPromo.enabled", false);
user_pref("browser.snippets.updateUrl", "");

//Запрещаем Веб Push-уведомления.
//(Даже если сайт не загружен и вы имели подписку от него, он мог вас уведомлять о выходе новой статьи или поступлении нового товара к примеру)
user_pref("dom.push.adaptive.enabled", false);
user_pref("dom.push.maxQuotaPerSubscription", 0);
user_pref("dom.push.udp.wakeupEnabled", false);
user_pref("dom.push.userAgentID", "");
user_pref("dom.push.connection.enabled", false);
user_pref("dom.push.serverURL", "");
user_pref("dom.serviceWorkers.openWindow.enabled", false);
user_pref("dom.webnotifications.serviceworker.enabled", false);

user_pref("dom.serviceWorkers.interception.enabled", false);
user_pref("dom.serviceWorkers.interception.opaque.enabled", false);
user_pref("dom.serviceWorkers.testUpdateOverOneDay", false);

//Не отправляем данные о загрузке страниц и переходах
//Забываем про сервис Pocket (Сервис отложенного чтения. Позволяет в дальнейшем просматривать материал с различных устройств)
user_pref("browser.pocket.api", "");
user_pref("browser.pocket.enabledLocales", "");
user_pref("browser.pocket.oAuthConsumerKey", "");
user_pref("browser.pocket.site", "");

//Отключение предзагрузки стартовой страницы и сохраняем анонимность при использовании миниатюр (плитки) на новой вкладке.
user_pref("browser.newtabpage.directory.source", "");
user_pref("browser.newtabpage.enabled", false);
user_pref("browser.newtabpage.introShown", false);

//Не использовать предзагрузку веб-страниц и dns-имён
//Закрываем доступ к камере, микрофону, проектору и другим устройствам.
user_pref("dom.gamepad.non_standard_events.enabled", false);
user_pref("dom.imagecapture.enabled", false);
user_pref("dom.presentation.discoverable", false);
user_pref("dom.presentation.discovery.enabled", false);
user_pref("dom.presentation.enabled", false);
user_pref("dom.presentation.tcp_server.debug", false);

//Отключает показ URL с описанием функций, для обладателей Windows 10
user_pref("browser.usedOnWindows10", true);
user_pref("browser.usedOnWindows10.introURL", "");

//Не будет замеряться время запуска браузера и не будет выводиться предупреждение, если он медленно стартует
//Не использовать Offline App Cache без явного разрешения со стороны пользователя
user_pref("offline-apps.allow_by_default", false);

//Уберем права выставленные по умолчанию. Из-за чего была возможность читать не которые данные указанные в about:support
user_pref("permissions.manager.defaultsUrl", "");

//Запрещаем соединение с устройством на Firefox OS для отладки по Wi-Fi
user_pref("devtools.remote.wifi.scan", false);
user_pref("devtools.remote.wifi.visible", false);

//Отключаем команду screenshot --imgur, с помощью которой можно было автоматически публиковать скриншоты на Imgur.com
user_pref("devtools.gcli.imgurClientID", "");
user_pref("devtools.gcli.imgurUploadURL", "");

//Отключаем SSDP, необходимый для обнаружения телевизора и видео-трансляции на него
//Не собираем статистику производительности декодирования HTML5-видео
//Защищаем нашу файловую систему. Чтобы ни кто не получил доступ к файлам
user_pref("device.storage.enabled", false);
user_pref("dom.caches.enabled", false);
user_pref("dom.fileHandle.enabled", false);

//Убираем подмену запрашиваемых страниц, на страницу провайдера
user_pref("captivedetect.maxRetryCount", 0);
user_pref("network.captive-portal-service.enabled", false);
user_pref("network.captive-portal-service.minInterval", 0);

//Передача firefox-ом информации о начале и окончании загрузки страницы и ее элементов
//Поддержка устройств виртуальной реальности
user_pref("dom.vr.cardboard.enabled", false);
user_pref("dom.vr.oculus.enabled", false);
user_pref("dom.vr.oculus050.enabled", false);

// Search Engines Update
user_pref("app.update.url.android", "");
user_pref("extensions.getAddons.showPane", false);
user_pref("extensions.webservice.discoverURL", "");
user_pref("extensions.webcompat-reporter.enabled", false);
user_pref("extensions.getAddons.discovery.api_url", "");
user_pref("extensions.abuseReport.enabled", false);
user_pref("webextensions.storage.sync.enabled", true);
user_pref("webextensions.storage.sync.serverURL", "");
user_pref("browser.search.isUS", true);
user_pref("canvas.capturestream.enabled", false);
user_pref("dom.vr.process.enabled", false);


// Automatically select "Offline Website Data" in the list of history items to clear.
user_pref("dom.allow_cut_copy", true);
user_pref("dom.presentation.device.name", "dummy-device");

//Domain Guessing intercepts the DNS "hostname not found" error, and resends the request to a guessed hostname that might use the correct domain
// Firefox and Chrome browsers have a privacy flaw where the user typed search terms are sent to DNS servers of ISPs
// 0 - never resolve single words, 1 - heuristic DNS resolve, 2 - always resolve.
user_pref("reader.parse-on-load.enabled", false);

//user_pref("layout.css.devPixelsPerPx", "1.5");
//user_pref("security.OCSP.enabled", 0);
user_pref("browser.slowStartup.samples", 0);
//0 - blank page, 1 - home page, 2 - last visited page, 3 - resume previous session.

// Turn off "Sends data to servers when leaving pages"
user_pref("beacon.enabled", false);

user_pref("dom.event.clipboardevents.enabled", false);

user_pref("device.sensors.enabled", false);
user_pref("device.sensors.motion.enabled", false);
user_pref("device.sensors.orientation.enabled", false);

user_pref("browser.send_pings",	false);
user_pref("browser.send_pings.require_same_host", true);
user_pref("dom.gamepad.enabled", false);
user_pref("dom.vr.enabled", false);
user_pref("dom.vibrator.enabled", false);
user_pref("webgl.min_capability_mode", true);
user_pref("webgl.disable-extensions", true);
user_pref("webgl.disable-fail-if-major-performance-caveat", true);
user_pref("webgl.enable-debug-renderer-info", false);
user_pref("dom.maxHardwareConcurrency",	2);
user_pref("javascript.options.wasm", false);
user_pref("clipboard.autocopy",	false);
user_pref("javascript.use_us_english_locale", true);
user_pref("keyword.enabled", false);
user_pref("browser.urlbar.trimURLs", false);
user_pref("browser.fixup.alternate.enabled", true); // "www."  ".com"

user_pref("browser.fixup.hide_user_pass", true);
user_pref("network.manage-offline-status", false);
user_pref("security.mixed_content.block_active_content", true);
user_pref("security.mixed_content.block_display_content", true);
user_pref("network.jar.open-unsafe-types", false);
user_pref("security.xpconnect.plugin.unrestricted", false);
user_pref("security.fileuri.strict_origin_policy", true);
user_pref("browser.urlbar.filter.javascript", true);
user_pref("javascript.options.asmjs", false);
user_pref("gfx.font_rendering.opentype_svg.enabled", false);
user_pref("browser.display.use_document_fonts",	0);
user_pref("network.protocol-handler.warn-external-default", true);
user_pref("network.protocol-handler.external.http", false);
user_pref("network.protocol-handler.external.https", false);
user_pref("network.protocol-handler.external.javascript", false);
user_pref("network.protocol-handler.external.moz-extension", false);
user_pref("network.protocol-handler.external.ftp", false);
user_pref("network.protocol-handler.external.file", false);
user_pref("network.protocol-handler.external.about", false);
user_pref("network.protocol-handler.external.chrome", false);
user_pref("network.protocol-handler.external.blob", false);
user_pref("network.protocol-handler.external.data", false);
user_pref("network.protocol-handler.expose-all", false);
user_pref("network.protocol-handler.expose.http", true);
user_pref("network.protocol-handler.expose.https", true);
user_pref("network.protocol-handler.expose.javascript", true);
user_pref("network.protocol-handler.expose.moz-extension", true);
user_pref("network.protocol-handler.expose.ftp", true);
user_pref("network.protocol-handler.expose.file", true);
user_pref("network.protocol-handler.expose.about", true);
user_pref("network.protocol-handler.expose.chrome", true);
user_pref("network.protocol-handler.expose.blob", true);
user_pref("network.protocol-handler.expose.data", true);
user_pref("security.dialog_enable_delay", 1000);
user_pref("xpinstall.signatures.required", true);
user_pref("lightweightThemes.update.enabled", false);
user_pref("plugin.state.flash",	1);
//user_pref("plugin.state.java", 0);
user_pref("dom.ipc.plugins.flash.subprocess.crashreporter.enabled", false);
user_pref("dom.ipc.plugins.reportCrashURL", false);
user_pref("plugin.state.libgnome-shell-browser-plugin",	0);
user_pref("plugins.click_to_play", true);

user_pref("places.history.enabled", false);
user_pref("signon.rememberSignons", false);
user_pref("browser.formfill.enable", false);
user_pref("network.cookie.lifetimePolicy", 2);
user_pref("signon.autofillForms", false);
user_pref("signon.formlessCapture.enabled", false);
user_pref("signon.autofillForms.http", false);
user_pref("security.insecure_field_warning.contextual.enabled", true);
user_pref("browser.formfill.expire_days", 0);
user_pref("browser.sessionstore.privacy_level",	2);
user_pref("browser.helperApps.deleteTempFileOnExit", true);
user_pref("browser.pagethumbnails.capturing_disabled", true);
user_pref("browser.shell.shortcutFavicons", false);
user_pref("browser.bookmarks.autoExportHTML", true);
user_pref("browser.bookmarks.max_backups", 3);
user_pref("browser.chrome.site_icons", false);
user_pref("security.insecure_password.ui.enabled", true);
user_pref("browser.download.folderList", 2);
user_pref("browser.newtab.url",	"about:blank");
user_pref("browser.newtabpage.activity-stream.feeds.snippets", false);
user_pref("browser.newtabpage.activity-stream.enabled", false);
user_pref("browser.newtabpage.enhanced", false);
user_pref("browser.newtab.preload", false);
user_pref("browser.newtabpage.directory.ping", "");
user_pref("network.IDN_show_punycode", true);
user_pref("browser.urlbar.autoFill", false);
user_pref("browser.urlbar.autoFill.typed", false);
user_pref("layout.css.visited_links_enabled", false);
user_pref("browser.urlbar.autocomplete.enabled", false);
user_pref("security.ask_for_password", 2);
user_pref("security.password_lifetime", 1);
user_pref("browser.offline-apps.notify", true);
user_pref("network.stricttransportsecurity.preloadlist", true);
user_pref("security.ssl.disable_session_identifiers", true);
user_pref("browser.download.manager.retention",	0);
user_pref("services.sync.prefs.sync.browser.sessionstore.restore_on_demand", false);
user_pref("browser.cache.disk.filesystem_reported", 1);
user_pref("browser.cache.disk.smart_size.first_run", false);
user_pref("browser.download.hide_plugins_without_extensions", false);
user_pref("browser.download.manager.addToRecentDocs", false);
user_pref("browser.download.panel.shown", true);
user_pref("browser.download.viewableInternally.typeWasRegistered.avif", true);
user_pref("browser.download.viewableInternally.typeWasRegistered.svg", true);
user_pref("browser.download.viewableInternally.typeWasRegistered.webp", true);
user_pref("browser.download.viewableInternally.typeWasRegistered.xml", true);
user_pref("browser.laterrun.bookkeeping.sessionCount", 37);
user_pref("browser.link.open_newwindow.restriction", 0);
user_pref("browser.messaging-system.whatsNewPanel.enabled", false);
user_pref("browser.newtabpage.activity-stream.showSponsored", false);
user_pref("browser.newtabpage.activity-stream.showSponsoredTopSites", false);
user_pref("browser.ping-centre.telemetry", false);
user_pref("browser.privacy.trackingprotection.menu", "private");
user_pref("browser.proton.toolbar.version", 3);
user_pref("browser.region.network.url", "");
user_pref("browser.region.update.enabled", false);
user_pref("browser.startup.homepage_override.mstone", "ignore");
user_pref("browser.tabs.remote.allowLinkedWebInFileUriProcess", false);
user_pref("browser.taskbar.lists.enabled", false);
user_pref("browser.taskbar.lists.frequent.enabled", false);
user_pref("browser.taskbar.lists.recent.enabled", false);
user_pref("browser.taskbar.lists.tasks.enabled", false);
user_pref("browser.toolbars.bookmarks.visibility", "always");
user_pref("browser.uitour.url", "");
user_pref("browser.urlbar.speculativeConnect.enabled", false);
user_pref("browser.urlbar.tipShownCount.searchTip_onboard", 4);
user_pref("browser.urlbar.tipShownCount.searchTip_redirect", 4);
user_pref("browser.urlbar.usepreloadedtopurls.enabled", false);
user_pref("browser.xul.error_pages.expert_bad_cert", true);
user_pref("captivedetect.canonicalURL", "");
user_pref("devtools.onboarding.telemetry.logged", false);
user_pref("dom.push.enabled", false);
user_pref("extensions.formautofill.addresses.enabled", false);
user_pref("extensions.formautofill.available", "off");
user_pref("extensions.formautofill.creditCards.available", false);
user_pref("extensions.formautofill.creditCards.enabled", false);
user_pref("extensions.formautofill.heuristics.enabled", false);
user_pref("extensions.htmlaboutaddons.recommendations.enabled", false);
user_pref("extensions.pendingOperations", false);
user_pref("extensions.pictureinpicture.enable_picture_in_picture_overrides", true);
user_pref("extensions.postDownloadThirdPartyPrompt", false);
user_pref("extensions.privatebrowsing.notification", true);
user_pref("extensions.systemAddon.update.url", "");
user_pref("extensions.ui.dictionary.hidden", true);
user_pref("extensions.ui.extension.hidden", false);
user_pref("extensions.ui.locale.hidden", false);
user_pref("gfx.font_rendering.graphite.enabled", false);
user_pref("intl.charset.fallback.override", "windows-1252");
user_pref("security.ssl.errorReporting.enabled", false);
user_pref("security.ssl.errorReporting.url", "");
user_pref("security.ssl.require_safe_negotiation", true);
user_pref("security.tls.enable_0rtt_data", false);
user_pref("toolkit.coverage.endpoint.base", "");
user_pref("toolkit.coverage.opt-out", true);
user_pref("toolkit.telemetry.bhrPing.enabled", false);
user_pref("toolkit.telemetry.coverage.opt-out", true);
user_pref("toolkit.telemetry.firstShutdownPing.enabled", false);
user_pref("toolkit.telemetry.newProfilePing.enabled", false);
user_pref("toolkit.telemetry.pioneer-new-studies-available", true);
user_pref("toolkit.telemetry.reportingpolicy.firstRun", false);
user_pref("toolkit.telemetry.server", "data:,");
user_pref("toolkit.telemetry.shutdownPingSender.enabled", false);
user_pref("toolkit.telemetry.updatePing.enabled", false);
user_pref("toolkit.winRegisterApplicationRestart", false);
user_pref("ui.prefersReducedMotion", 1);


//-------------------------------------------------------------------------------------------
//*******************************************************************************************

// Custom user preferences
user_pref("browser.newtabpage.activity-stream.asrouter.userprefs.cfr", false);
user_pref("browser.newtabpage.activity-stream.feeds.section.highlights", false);
user_pref("browser.newtabpage.activity-stream.section.highlights.includePocket", false);
user_pref("browser.newtabpage.activity-stream.section.highlights.includeBookmarks", false);
user_pref("browser.newtabpage.activity-stream.section.highlights.includeDownloads", false);
user_pref("browser.newtabpage.activity-stream.section.highlights.includeVisited", false);
user_pref("browser.newtabpage.activity-stream.feeds.section.topstories", false);
user_pref("browser.newtabpage.activity-stream.showSearch", false);
user_pref("browser.tabs.crashReporting.sendReport", false);
user_pref("browser.crashReports.unsubmittedCheck.enabled", false);
user_pref("browser.slowStartup.notificationDisabled", true);
user_pref("browser.slowStartup.maxSamples", 0);
user_pref("browser.uitour.enabled", false);
user_pref("browser.discovery.enabled", false);
//don't show EULA
user_pref("browser.rights.3.shown", true);

user_pref("browser.urlbar.suggest.searches", false);
user_pref("browser.urlbar.suggest.history", false);

user_pref("browser.urlbar.groupLabels.enabled", false);
user_pref("browser.casting.enabled", false);

user_pref("browser.aboutHomeSnippets.updateUrl", "");
user_pref("browser.search.update", false);
user_pref("browser.startup.blankWindow", false);
user_pref("browser.startup.homepage", "");
user_pref("browser.urlbar.suggest.openpage", false);
user_pref("browser.newtabpage.activity-stream.feeds.telemetry", false);
user_pref("browser.contentblocking.rejecttrackers.reportBreakage.enabled", false);
user_pref("browser.contentblocking.reportBreakage.enabled", false);
//user_pref("browser.contentblocking.introCount", 99);
user_pref("browser.onboarding.enabled", false);
user_pref("browser.search.openintab", true);

user_pref("browser.pocket.enabled", false);
user_pref("extensions.pocket.enabled", false);

user_pref("devtools.webide.enabled", false);
user_pref("devtools.webide.autoinstallADBHelper", false);
user_pref("devtools.webide.autoinstallFxdtAdapters", false);
user_pref("devtools.debugger.remote-enabled", false);
user_pref("devtools.chrome.enabled", false);
user_pref("devtools.debugger.force-local", true);
user_pref("devtools.browserconsole.filter.serviceworkers", false);
user_pref("devtools.devedition.promo.url", "");
user_pref("devtools.telemetry.supported_performance_marks", "");
user_pref("devtools.webconsole.filter.serviceworkers", false);

user_pref("toolkit.telemetry.enabled", false);
user_pref("toolkit.telemetry.unified", false);
user_pref("toolkit.telemetry.archive.enabled", false);
user_pref("toolkit.telemetry.hybridContent.enabled", false);
//disable perf monitoring prompt
user_pref("toolkit.telemetry.prompted", 2);
//disable perf. monitoring
user_pref("toolkit.cosmeticAnimations.enabled", false);
user_pref("toolkit.datacollection.infoURL", "");
user_pref("toolkit.crashreporter.infoURL", "");

user_pref("app.feedback.baseURL", "");

user_pref("breakpad.reportURL",	"");
user_pref("dom.flyweb.enabled", false);
user_pref("dom.battery.enabled", false);

user_pref("pdfjs.disabled", true);
user_pref("loop.logDomains", false);

user_pref("signon.generation.enabled", false);
user_pref("signon.management.page.breach-alerts.enabled", false);

// Social
user_pref("social.directories", "");
user_pref("social.remote-install.enabled", false);
user_pref("social.toast-notifications.enabled", false);
user_pref("social.whitelist", "");

//virtual reality
user_pref("permissions.default.xr", 2);

user_pref("full-screen-api.warning.delay", 0);
user_pref("full-screen-api.warning.timeout", 2500);

// GFX
user_pref("gfx.use_text_smoothing_setting", true);
user_pref("gfx.webrender.enabled", true);
user_pref("gfx.webrender.highlight-painted-layers", false);

// [performance gain in openGL and HW compositing]
user_pref("layers.acceleration.force-enabled", true);

user_pref("browser.newtabpage.activity-stream.asrouter.devtoolsEnabled", false);
user_pref("browser.newtabpage.activity-stream.asrouter.providers.cfr", "");
user_pref("browser.newtabpage.activity-stream.asrouter.providers.cfr-fxa", "");
user_pref("browser.newtabpage.activity-stream.asrouter.providers.message-groups", "");
user_pref("browser.newtabpage.activity-stream.asrouter.providers.messaging-experiments", "");
user_pref("browser.newtabpage.activity-stream.asrouter.providers.onboarding", "");
user_pref("browser.newtabpage.activity-stream.asrouter.providers.snippets", "");
//user_pref("browser.newtabpage.activity-stream.asrouter.providers.whats-new-panel", "");
user_pref("browser.newtabpage.activity-stream.asrouter.useRemoteL10n", false);
//user_pref("browser.newtabpage.activity-stream.default.sites", "");
user_pref("browser.newtabpage.activity-stream.discoverystream.config", "");
user_pref("browser.newtabpage.activity-stream.discoverystream.enabled", false);
user_pref("browser.newtabpage.activity-stream.discoverystream.endpointSpocsClear", "");
user_pref("browser.newtabpage.activity-stream.discoverystream.endpoints", "");
user_pref("browser.newtabpage.activity-stream.discoverystream.engagementLabelEnabled", false);
user_pref("browser.newtabpage.activity-stream.discoverystream.flight.blocks", "");
user_pref("browser.newtabpage.activity-stream.discoverystream.hardcoded-basic-layout", false);
user_pref("browser.newtabpage.activity-stream.discoverystream.isCollectionDismissible", false);
user_pref("browser.newtabpage.activity-stream.discoverystream.personalization.modelKeys", "");
user_pref("browser.newtabpage.activity-stream.discoverystream.rec.impressions", "");
user_pref("browser.newtabpage.activity-stream.discoverystream.recs.personalized", false);
user_pref("browser.newtabpage.activity-stream.discoverystream.region-basic-layout", false);
user_pref("browser.newtabpage.activity-stream.discoverystream.region-layout-config", "");
user_pref("browser.newtabpage.activity-stream.discoverystream.region-spocs-config", "");
user_pref("browser.newtabpage.activity-stream.discoverystream.region-stories-config", "");
user_pref("browser.newtabpage.activity-stream.discoverystream.spoc.impressions", "");
user_pref("browser.newtabpage.activity-stream.discoverystream.spocs-endpoint", "");
user_pref("browser.newtabpage.activity-stream.discoverystream.spocs.personalized", false);
user_pref("browser.newtabpage.activity-stream.feeds.aboutpreferences", false);
user_pref("browser.newtabpage.activity-stream.feeds.asrouterfeed", false);
user_pref("browser.newtabpage.activity-stream.feeds.discoverystreamfeed", false);
user_pref("browser.newtabpage.activity-stream.feeds.favicon", false);
user_pref("browser.newtabpage.activity-stream.feeds.newtabinit", false);
user_pref("browser.newtabpage.activity-stream.feeds.places", false);
user_pref("browser.newtabpage.activity-stream.feeds.prefs", false);
user_pref("browser.newtabpage.activity-stream.feeds.recommendationproviderswitcher", false);
user_pref("browser.newtabpage.activity-stream.feeds.section.topstories.options", "");
user_pref("browser.newtabpage.activity-stream.feeds.sections", false);
user_pref("browser.newtabpage.activity-stream.feeds.system.topsites", false);
user_pref("browser.newtabpage.activity-stream.feeds.system.topstories", false);
user_pref("browser.newtabpage.activity-stream.feeds.systemtick", false);
user_pref("browser.newtabpage.activity-stream.feeds.topsites", false);
user_pref("browser.newtabpage.activity-stream.filterAdult", false);
user_pref("browser.newtabpage.activity-stream.fxaccounts.endpoint", "");
user_pref("browser.newtabpage.activity-stream.impressionId", "");


user_pref("gecko.handlerService.schemes.irc.0.name", "");
user_pref("gecko.handlerService.schemes.irc.0.uriTemplate", "");
user_pref("gecko.handlerService.schemes.ircs.0.name", "");
user_pref("gecko.handlerService.schemes.ircs.0.uriTemplate", "");
user_pref("gecko.handlerService.schemes.mailto.0.name", "");
user_pref("gecko.handlerService.schemes.mailto.0.uriTemplate", "");
user_pref("gecko.handlerService.schemes.mailto.1.name", "");
user_pref("gecko.handlerService.schemes.mailto.1.uriTemplate", "");


user_pref("network.connectivity-service.DNSv4.domain", "");
user_pref("network.connectivity-service.DNSv6.domain", "");
user_pref("network.connectivity-service.enabled", false);

user_pref("security.identitypopup.recordEventTelemetry", false);
user_pref("security.protectionspopup.recordEventTelemetry", false);
//user_pref("security.remote_settings.crlite_filters.bucket", "");
//user_pref("security.remote_settings.crlite_filters.collection", "");
//user_pref("security.remote_settings.crlite_filters.signer", "");
user_pref("security.remote_settings.intermediates.bucket", "");
user_pref("security.remote_settings.intermediates.collection", "");
user_pref("security.remote_settings.intermediates.enabled", false);
user_pref("security.remote_settings.intermediates.signer", "");
user_pref("security.ssl.enable_false_start", false);


user_pref("toolkit.coverage.enabled", false);
user_pref("toolkit.legacyUserProfileCustomizations.stylesheets", true);
user_pref("toolkit.telemetry.debugSlowSql", false);
user_pref("toolkit.telemetry.ecosystemtelemetry.enabled", false);
user_pref("toolkit.telemetry.geckoview.streaming", false);
user_pref("toolkit.telemetry.isGeckoViewMode", false);
//user_pref("toolkit.telemetry.previousBuildID", "");
//user_pref("toolkit.telemetry.server_owner", "");
user_pref("toolkit.telemetry.shutdownPingSender.enabledFirstSession", false);
user_pref("toolkit.telemetry.testing.overrideProductsCheck", false);

user_pref("browser.contentblocking.reportBreakage.url", "");

user_pref("extensions.pocket.api", "");

user_pref("identity.fxaccounts.remote.oauth.uri", "");
user_pref("identity.fxaccounts.remote.pairing.uri", "");
user_pref("identity.fxaccounts.remote.root", "");
user_pref("identity.fxaccounts.service.monitorLoginUrl", "");

//security.certerrors.mitm.priming.endpoint	https://mitmdetection.services.mozilla.com/



