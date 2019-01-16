$url = "https://data.kingcounty.gov/resource/u7jg-ujbh.json"
$apptoken = "Qi_Hw3GSTkt3hUGw5fVttszy5jo84xlcPr24"

# Set header to accept JSON
$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$headers.Add("Accept","application/json")
$headers.Add("X-App-Token",$apptoken)

$results = Invoke-RestMethod -Uri $url -Method get -Headers $headers