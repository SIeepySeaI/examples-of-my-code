"use strict";
var dataFromUser = [];
var ConvertTime;
var ConvertHour;
var ConvertMinute;
function SaveParam() {
  //имя контроллера
  dataFromUser[0] = parseInt(pickController,10);
  if(document.getElementById("sideMenu").style.width === "300px"){
    dataFromUser[4] = parseInt(document.getElementById("0_TempControlInput").value,10);
  }
  else{
    dataFromUser[4] = parseInt(document.getElementById("1_TempControlInput").value,10);
  }
  
  for (let i = 1; i <= 2; i++) {
    if(document.getElementById("sideMenu").style.width === "300px"){
      if(document.getElementById(i + "Mod").checked)
      dataFromUser[5] = i;
    }
    else{
    if(document.getElementById(i + "ModFull").checked)
    dataFromUser[5] = i;
    }
  }
  for (let i = 0; i <= 3; i++) {
    if(document.getElementById("sideMenu").style.width === "300px"){
      if(document.getElementById(i + "Fan").checked)
      dataFromUser[6] = i;
    }
    else{
      if(document.getElementById(i + "FanFull").checked)
      dataFromUser[6] = i;
    }
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
  //Проверка режима работы по периодам и активного поддержания температуры
    if (document.getElementById("4SysStatFull").checked) {
      if (document.getElementById("1PeriodFull").checked) {
        dataFromUser[15] = 5;
      }
      else if (document.getElementById("0PeriodFull").checked) {
        dataFromUser[15] = 4;
      }
      
    }
    else if (document.getElementById("6SysStatFull").checked) {
      if (document.getElementById("1PeriodFull").checked) {
        dataFromUser[15] = 7;
      }
      else if (document.getElementById("0PeriodFull").checked) {
        dataFromUser[15] = 6;
      }
    }
  
  ConvertTime = document.getElementById("17_16_TimeInput").value;
  if (ConvertTime =="") {
    dataFromUser[17] = -2;
    dataFromUser[16] = -2;
  }
  else{
    dataFromUser[17] = parseInt(ConvertTime.substr(0,2),10);
    dataFromUser[16] = parseInt(ConvertTime.substr(3),10);
  }
  dataFromUser[18] = document.getElementById("WeekSelectFull").selectedIndex;
  for (let i = 19; i <= 30; i++) {
    ConvertTime = document.getElementById(i + "_TimeInput").value;
    if (ConvertTime =="") {
      dataFromUser[i] = -2;
    }
    else{
      ConvertHour = parseInt(ConvertTime.substr(0,2),10);
      ConvertMinute = parseInt(ConvertTime.substr(3),10);
      dataFromUser[i] =  (ConvertHour + ConvertMinute/60)*4;
    }
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
    success: SuccessRecieve,
    error: function(jqxhr, status, errorMsg) {
      console.log("Статус: " + status + " Ошибка: " + errorMsg);
    }
  });
}
function SuccessRecieve(data) {
  alert("Данные успешно сохранены");
  document.getElementById("17_16_TimeInput").value ='';
  for (let i = 19; i <= 30; i++) {
    ConvertTime = document.getElementById(i + "_TimeInput").value ='';
  }
  document.getElementById("overlayMenu").style.width = "0%";
  //console.log(data);
}