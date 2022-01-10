"# Windows 10 Security and Privacy"<br> 
<br>
result_dnsf.txt 163MB split into two files:<br>
multi-point-dns-result-filtered.txt - 98MB<br> 
and single-dns-one-point-result-filtered.txt - 64MB<br>
DNSTree convert 98MB to 1.2MB(wildcard-dns-statistics-processed-result-dict-distinct.txt)<br>
with wildcard rules based on statistics(domain.tld) or Dns Tree Level 2 <br>
<br>
Root - 0<br>
|<br>
com - 1<br>
|<br>
statdynamic.com - 2<br>
|<br>
dd.statdynamic.com - 3<br>
<br>
*.nanopool.org<br>
*.coinlab.biz<br>
*.statdynamic.com<br>
*.adx[0-9].com<br>
<br>
for dnscrypt-proxy-2.1.1<br>
data 1.2MB(wildcard-dns-statistics-processed-result-dict-distinct.txt) for insert into blocked-names.txt file<br>
