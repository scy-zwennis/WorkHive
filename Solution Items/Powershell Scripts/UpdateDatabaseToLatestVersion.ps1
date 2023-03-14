function WriteLine($message){
	write-host "";
	write-host -ForegroundColor DarkGreen $message;
	write-host "";
}

$StartDt          	= get-date;
$DatabaseName 		= "WorkHive";
$ConnectionString 	= "`"Data Source=localhost;Initial Catalog=$DatabaseName;Integrated Security=SSPI`"";

WriteLine("UPDATE DATABASE TO LATEST VERSION SCRIPT STARTED: "+$StartDt);

$ScriptPath 		= "..\Sql Scripts\Rollout\";
..\Tools\WorkHive.DatabaseUpdater\executable\WorkHive.DatabaseUpdater.exe -Database="$DatabaseName" -ConnectionString="$ConnectionString" -ScriptPath="$ScriptPath";
if ($LASTEXITCODE -ne 0)
{
	Exit $LASTEXITCODE;
}

$EndDt            = get-date;
[string]$Timespan = ($EndDt) - ($StartDt);
WriteLine("UPDATE DATABASE TO LATEST VERSION SCRIPT COMPLETED: "+$EndDt);
WriteLine("TIME ELAPSED: "+$Timespan);