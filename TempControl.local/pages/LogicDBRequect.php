<?php
// Устанавливаем соединение с базой данных указывая ip, логин, пароль и имя БД
$link = mysqli_connect('0.0.0.0','****','****','****');
//Если не удается соедениться - выводит код ошибки
if (mysqli_connect_errno()) {
    echo 'Connection failed:('.mysqli_connect_errno().'): '.mysqli_connect_error();
    exit();
}
    // SQL запрос
    $sql = "SELECT * FROM administration_term_control_service";
    //Делаем запрос и принимаем ответ
    $result = mysqli_query($link, $sql);
    $Text;
    //Получаем из ответа значения ячеек
    $categories = mysqli_fetch_all($result);
    echo json_encode ($categories);
    //echo json_encode ("AJAX работает");
?>