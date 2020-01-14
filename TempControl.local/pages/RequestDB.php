<?php
// Устанавливаем соединение с базой данных указывая ip, логин, пароль и имя БД
$link = mysqli_connect('0.0.0.0','****','****','****');
//Если не удается соедениться - выводит код ошибки
if (mysqli_connect_errno()) {
    echo 'Connection failed:('.mysqli_connect_errno().'): '.mysqli_connect_error();
    exit();
}
//Функция с запросом принимает переменную с соеденением БД
function MysqlRequest(){
    global $link;
    // SQL запрос
    $sql = "SELECT * FROM tempcontroller";
    //Делаем запрос и принимаем ответ
    $result = mysqli_query($link, $sql);
    // echo '<pre>';
    // var_dump($categories);
    // echo '</pre>';
    //Получаем из ответа значения ячеек
    $categories = mysqli_fetch_all($result, MYSQLI_ASSOC);
    return $categories;
}
?>