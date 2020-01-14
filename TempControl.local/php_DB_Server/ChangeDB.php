<?php
//Массив для цикла
$CommandString;
$NameColumns = [
  0 => "id",
  1 => "Type",
  2 => "WorkingArea",
  3 => "TempNow",
  4 => "SetTemp",
  5 => "WorkingMode",
  6 => "FanMode",
  7 => "SystemMode",
  8 => "OffsetTemp",
  9 => "MaxSetTemp",
  10 => "MinSetTemp",
  11 => "VoltOutIncrease",
  12 => "VoltageOut",
  13 => "AntifreezeTempSet",
  14 => "AntifreezeType",
  15 => "WorkStatus",
  16 => "TimeMinutes",
  17 => "TimeHours",
  18 => "TimeWeek",
  19 => "WorkWeek1_ZoneTimeStart",
  20 => "WorkWeek2_ZoneTimeStart",
  21 => "WorkWeek3_ZoneTimeStart",
  22 => "WorkWeek4_ZoneTimeStart",
  23 => "Saturday1_ZoneTimeStart",
  24 => "Saturday2_ZoneTimeStart",
  25 => "Saturday3_ZoneTimeStart",
  26 => "Saturday4_ZoneTimeStart",
  27 => "Sunday1_ZoneTimeStart",
  28 => "Sunday2_ZoneTimeStart",
  29 => "Sunday3_ZoneTimeStart",
  30 => "Sunday4_ZoneTimeStart"
];
//Просто вернуть json обратно
//$jsonfile = file_get_contents("php://input");
//echo($jsonfile);
//декодим принимаемый json в массив
$jsonfile = json_decode(file_get_contents("php://input"),true);

$link = mysqli_connect('192.168.64.201','root','0n0gktybt','term_control');
$CommandString = "";
$KeyString = "";
define('NumKey','1');
for ($i=4; $i <= 30; $i++) { 
  //Если в данные не изменяли, то пропускаем
  if ($jsonfile[$i]>-1) {
    $CommandString .= " ".$NameColumns[$i]."=".$jsonfile[$i].",";
    $KeyString .= " ".$NameColumns[$i]."=".NumKey.",";
  }
}
$CommandString = substr($CommandString,0,-1);
$KeyString = substr($KeyString,0,-1);
$sql = "UPDATE controllers_data_update SET ".$CommandString." WHERE id = " . $jsonfile[0];
$result = mysqli_query($link, $sql);
$sql = "UPDATE controllers_key_update SET ".$KeyString." WHERE id = " . $jsonfile[0];
$result = mysqli_query($link, $sql);
  echo $result;
mysqli_close( $link );
?>