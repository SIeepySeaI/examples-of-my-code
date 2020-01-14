"use strict";
var dataFromUser = [];
var ConvertTime;
var ConvertHour;
var ConvertMinute;
function SaveParam() {
  //имя контроллера
  dataFromUser[0] = parseInt(pickController,10);
  dataFromUser[4] = parseInt(document.getElementById("1_TempControlInput").value,10);
  for (let i = 1; i <= 2; i++) {
    if(document.getElementById(i + "ModFull").checked)
    dataFromUser[5] = i;
  }
  for (let i = 0; i <= 3; i++) {
    if(document.getElementById(i + "FanFull").checked)
    dataFromUser[6] = i;
  }
  for (let i = 0; i <= 2; i++) {
    if(document.getElementById(i + "SysModFull").checked)
    dataFromUser[7] = i;
  }
  //Получаем значения со слайдеров с температурными данными
  dataFromUser[8] = parseInt(document.getElementById("2_TempControlInput").value,10);
  dataFromUser[9] = parseInt(document.getElementById("3_TempControlInput").value,10);
  dataFromUser[10] = parseInt(document.getElementById("4_TempControlInput").value,10);
  dataFromUser[13] = parseInt(document.getElementById("5_TempControlInput").value,10);
  for (let i = 0; i <= 2; i++) {
    if(document.getElementById(i + "AntifreezeType").checked)
    dataFromUser[14] = i;
  }
  ConvertTime = document.getElementById("17_16_TimeInput").value;
  dataFromUser[17] = parseInt(ConvertTime.substr(0,2),10);
  dataFromUser[16] = parseInt(ConvertTime.substr(3),10);
  dataFromUser[18] = document.getElementById("WeekSelectFull").selectedIndex;
  for (let i = 19; i <= 30; i++) {
    ConvertTime = document.getElementById(i + "_TimeInput").value;
    ConvertHour = parseInt(ConvertTime.substr(0,2),10);
    ConvertMinute = parseInt(ConvertTime.substr(3),10);
      dataFromUser[i] =  (ConvertHour + ConvertMinute/60)*4;
  }
  // Проверяем коректность ввода времени работы
  for (let i = 19; i <= 30; i++) {
    if (dataFromUser[i]%1!=0) {
      alert("Проверьте правильность введенного времени включения-отключения! Минуты должны быть кратны 15.");
      return;
    }
  }
  //Проверяем совпадают ли данные со старыми данными
  //и заполняем неизменяемые ячейки данных
   for (let i = 1; i <= 30; i++) {
     if (dataFromUser[i] == undefined) {
       dataFromUser[i] = -2;
     }
     else if(dataFromUser[i]==dataFromDB[pickController][i]){
       dataFromUser[i] = -2;
     }
   }
  // for (let i = 0; i <= 30; i++) {
  //    console.log(dataFromUser[i] +" "+ i);
  // }
  //преобразуем массив в json
  let jsonArray = JSON.stringify(dataFromUser);
  //console.log(jsonArray);
  $.ajax({
    type: 'POST',
    url: '../pages/ChangeDB.php',
    data: jsonArray,
    dataType: 'json',
    success: SuccessRecieve
  });
}
function SuccessRecieve(data) {
  alert(data);
  //console.log(data);
}