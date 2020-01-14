"use strict";
var pickController = 1;
var dataFromDB = [];
var TimeConvertingHour;
var TimeConvertingMinute;
timerOut();
function timerOut() {
  //запрос PHP файлу сервере
  $.ajax ({
    url: "../pages/TestResponse.php",
    type: "POST",
    dataType: "html",
    success: funcSuccess
  });
  
}
  $(document).ready (function (){
    //задаём таймер опроса сервера
      let timerId = setInterval(timerOut, 5000);
  });
function funcSuccess(data) {
  //принимаем json-представление массива и перегоняем его javascript-массив
  data = JSON.parse(data);
  for(var x in data) { 
    if (data.hasOwnProperty(x))
    dataFromDB[x] = [];
    for(var y in data[x]) {
      if (data[x].hasOwnProperty(y)) {
        dataFromDB[x][y] = data[x][y];
      }
    }
  }
  // updateMenuData(pickController);
  console.log(dataFromDB);
  console.log("its work!");
}
//отображение данных по выбранному контроллеру
function clickController(item){
  pickController = item.className.substr(item.className.length - 1);
  console.log(pickController);
  document.getElementById("1_TempControlInput").value = document.getElementById("0_TempControlInput").value = dataFromDB[pickController][4];
  document.getElementById("1_InputValue").innerText = "";
  document.getElementById("2_TempControlInput").value = dataFromDB[pickController][8];
  document.getElementById("2_InputValue").innerText = "";
  document.getElementById("3_TempControlInput").value = dataFromDB[pickController][9];
  document.getElementById("3_InputValue").innerText = "";
  document.getElementById("4_TempControlInput").value = dataFromDB[pickController][10];
  document.getElementById("4_InputValue").innerText = "";
  document.getElementById("5_TempControlInput").value = dataFromDB[pickController][13];
  document.getElementById("5_InputValue").innerText = "";
  updateMenuData(pickController);
}
//заполняем меню данными 
function updateMenuData(indexItemController){
  document.getElementById("2_sideMenuItem").innerText = document.getElementById("2_TempControlData").innerText = dataFromDB[indexItemController][2];
  document.getElementById("3_sideMenuItem").innerText = document.getElementById("3_TempControlData").innerText = dataFromDB[indexItemController][3] / 2 + "°С";
  document.getElementById("4_sideMenuItem").innerText = document.getElementById("4_TempControlData").innerText =  dataFromDB[indexItemController][4] / 2 + "°С";
  //Режим охлаждения/нагрева
  for (let i = 1; i <= 2; i++) {
    if(dataFromDB[indexItemController][5]==i){
    document.getElementById(i + "Mod").checked = document.getElementById(i + "ModFull").checked = "checked";
    }
  }
  //Режим работы вентиляторов
  for (let i = 0; i <= 3; i++) {
    if(dataFromDB[indexItemController][6]==i){
      document.getElementById(i + "Fan").checked = document.getElementById(i + "FanFull").checked = "checked";
    }
  }
  //заполняем оставшиеся пункты побробного меню
  //Блокировка режимов
  for (let i = 0; i <= 2; i++) {
    if(dataFromDB[indexItemController][7]==i){
      document.getElementById(i + "SysModFull").checked = "checked";
    }
  }

  document.getElementById("8_TempControlData").innerText = dataFromDB[indexItemController][8] - 16 + "°С";
  document.getElementById("9_TempControlData").innerText = dataFromDB[indexItemController][9]/2 + "°С";
  document.getElementById("10_TempControlData").innerText = dataFromDB[indexItemController][10]/2 + "°С";
  document.getElementById("11_TempControlData").innerText = dataFromDB[indexItemController][11]/2 + "°С";
  document.getElementById("12_TempControlData").innerText = dataFromDB[indexItemController][12];
  document.getElementById("13_TempControlData").innerText = dataFromDB[indexItemController][13] + "°С";
  for (let i = 0; i <= 2; i++) {
    if(dataFromDB[indexItemController][14]==i){
      document.getElementById(i + "AntifreezeType").checked = "checked";
    }
  }
  //Тех статус контроллера(в доработке)
  document.getElementById("15_TempControlData").innerText = dataFromDB[indexItemController][15];
  //Время в минутах и часах
  document.getElementById("17_16_TempControlData").innerText = dataFromDB[indexItemController][17] + ":" + dataFromDB[indexItemController][16];
  for (let i = 0; i < 6; i++) {
    if (dataFromDB[indexItemController][18]==i) {
      document.getElementById("WeekSelectFull").value = i;
    }
  }
  // Время включения и отключения
  for (let i = 19; i <= 30; i++) {
    TimeConvertingHour = (dataFromDB[indexItemController][i]) * 0.25;
    TimeConvertingMinute = TimeConvertingHour % 1;
    TimeConvertingHour = TimeConvertingHour - TimeConvertingMinute;
    TimeConvertingMinute = TimeConvertingMinute * 60;
    if (TimeConvertingMinute < 10) {
    TimeConvertingMinute = "0" + TimeConvertingMinute;
    }
    document.getElementById(i + "_TempControlData").innerText = TimeConvertingHour + ":" + TimeConvertingMinute;
  }
  
}