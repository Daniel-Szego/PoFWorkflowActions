
Add-Type -Path 'C:\Windows\Microsoft.NET\assembly\GAC_MSIL\Microsoft.Office.Word.Server\v4.0_15.0.0.0__71e9bce111e9429c\Microsoft.Office.Word.Server.dll'
$jobSettings = New-Object Microsoft.Office.Word.Server.Conversions.ConversionJobSettings
$jobSettings.OutputFormat = "PDF"
$job = New-Object Microsoft.Office.Word.Server.Conversions.ConversionJob("WAS", $jobSettings)
$job.UserToken = (Get-SPWeb "http://demo.mp.local/profiles/dev/").CurrentUser.UserToken
$job.AddFile("http://demo.mp.local/profiles/dev/Shared%20Documents/xxx.docx", "http://demo.mp.local/profiles/dev/Shared%20Documents/Test.pdf")
$job.Start()
Start-SPTimerJob "WAS"
new-object Microsoft.Office.Word.Server.Conversions.ConversionJobStatus("WAS", $job.JobId,$null)
