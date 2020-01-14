<?php
// Устанавливаем соединение с базой данных указывая ip, логин, пароль и имя БД
$link = mysqli_connect('192.168.64.201','root','0n0gktybt','term_control');
// $NameColumns = [
//     0 => "id",
//     1 => "Type",
//     2 => "WorkingArea",
//     3 => "TempNow",
//     4 => "SetTemp",
//     5 => "WorkingMode",
//     6 => "FanMode",
//     7 => "SystemMode",
//     8 => "OffsetTemp",
//     9 => "MaxSetTemp",
//     10 => "MinSetTemp",
//     11 => "VoltOutIncrease",
//     12 => "VoltageOut",
//     13 => "AntifreezeTempSet",
//     14 => "AntifreezeType",
//     15 => "WorkStatus",
//     16 => "TimeMinutes",
//     17 => "TimeHours",
//     18 => "TimeWeek",
//     19 => "Week(1-5)1_ZoneTimeStart",
//     20 => "Week(1-5)2_ZoneTimeStart",
//     21 => "Week(1-5)3_ZoneTimeStart",
//     22 => "Week(1-5)4_ZoneTimeStart",
//     23 => "Saturday1_ZoneTimeStart",
//     24 => "Saturday2_ZoneTimeStart",
//     25 => "Saturday3_ZoneTimeStart",
//     26 => "Saturday4_ZoneTimeStart",
//     27 => "Sunday1_ZoneTimeStart",
//     28 => "Sunday2_ZoneTimeStart",
//     29 => "Sunday3_ZoneTimeStart",
//     30 => "Sunday4_ZoneTimeStart"
// ];
//Если не удается соедениться - выводит код ошибки
if (mysqli_connect_errno()) {
    echo 'Connection failed:('.mysqli_connect_errno().'): '.mysqli_connect_error();
    exit();
}
    // SQL запрос
    $sql = "SELECT * FROM controllers_data";
    //Делаем запрос и принимаем ответ
    $result = mysqli_query($link, $sql);
    $Text;
    // echo '<pre>';
    // var_dump($categories);
    // echo '</pre>';
    //Получаем из ответа значения ячеек
    $categories = mysqli_fetch_all($result);
    echo json_encode ($categories);
    mysqli_close( $link );
?>