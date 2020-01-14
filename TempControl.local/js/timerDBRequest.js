"use strict";
var pickController = 1;
var workshopData = [];
var adminData = [];
var adminLogicData = [];
var updateTableData = [];
var TimeConvertingHour;
var TimeConvertingMinute;
var roundUp;
timerOut();



function timerOut() {
  //запрос PHP файлу сервере
  $.ajax({
    url: "../pages/TestResponse.php",
    type: "POST",
    dataType: "html",
    success: funcSuccess
  });
  $.ajax({
    url: "../pages/AdminDBRequest.php",
    type: "POST",
    dataType: "html",
    success: funcSuccessAdmin
  });
  $.ajax({
    url: "../pages/LogicDBRequect.php",
    type: "POST",
    dataType: "html",
    success: funcSuccessAdminLogic
  });
}
$(document).ready(function () {
  //задаём таймер опроса сервера
  let timerId = setInterval(timerOut, 5000);
});

function funcSuccess(data) {
  //принимаем json-представление массива и перегоняем его javascript-массив
  data = JSON.parse(data);
  for (var x in data) {
    if (data.hasOwnProperty(x))
      workshopData[x] = [];
    for (var y in data[x]) {
      if (data[x].hasOwnProperty(y)) {
        workshopData[x][y] = data[x][y];
      }
    }
  }
  // updateMenuData(pickController);
  console.log(workshopData);
  console.log("Workshop data was read sucsesfull");
  updateMenuData(pickController,1);
}

function funcSuccessAdmin(data) {
  //принимаем json-представление массива и перегоняем его javascript-массив
  data = JSON.parse(data);
  for (var x in data) {
    if (data.hasOwnProperty(x))
      adminData[x] = [];
    for (var y in data[x]) {
      if (data[x].hasOwnProperty(y)) {
        adminData[x][y] = data[x][y];
      }
    }
  }
  // updateMenuData(pickController);
  console.log(adminData);
  console.log("Administration logic data was read sucsesfull");
  updateMenuData(pickController,2);
}
function funcSuccessAdminLogic(data) {
  //принимаем json-представление массива и перегоняем его javascript-массив
  data = JSON.parse(data);
  for (var x in data) {
    if (data.hasOwnProperty(x))
    adminLogicData[x] = [];
    for (var y in data[x]) {
      if (data[x].hasOwnProperty(y)) {
        adminLogicData[x][y] = data[x][y];
      }
    }
  }
  // updateMenuData(pickController);
  console.log(adminLogicData);
  console.log("Administration data was read sucsesfull");
  updateMenuData(pickController,3);
}
//отображение данных по выбранному контроллеру
function clickController(item) {
  if (document.getElementById("sideMenu").style.width !== "300px") {
    document.getElementById("MenuSelecror").classList.toggle("change");
    document.getElementById("sideMenu").style.width = "300px";
    document.getElementById("main").style.marginLeft = "300px";
  }
  //Проверяем выбран ли элемет из административного здания
  if (item.className == "Floor") {
    console.log("Администрация");
    pickController = item.id.substr(item.className.length - 1);
    console.log(pickController);
    updateMenuData(pickController,1);
  } else {
    pickController = item.className.substr(item.className.length - 1);
    console.log(pickController);
    document.getElementById("1_TempControlInput").value = document.getElementById("0_TempControlInput").value = workshopData[pickController][4];
    document.getElementById("1_InputValue").innerText = "";
    document.getElementById("2_TempControlInput").value = workshopData[pickController][8];
    document.getElementById("2_InputValue").innerText = "";
    document.getElementById("3_TempControlInput").value = workshopData[pickController][9];
    document.getElementById("3_InputValue").innerText = "";
    document.getElementById("4_TempControlInput").value = workshopData[pickController][10];
    document.getElementById("4_InputValue").innerText = "";
    document.getElementById("5_TempControlInput").value = workshopData[pickController][13];
    document.getElementById("5_InputValue").innerText = "";
    updateMenuData(pickController,1);
  }
}
//заполняем меню данными 
//updateTable: 1 - Таблица с данными по цеху, 2 - административное здание, 3 - данные для логики по административному зданию
function updateMenuData(indexItemController, updateTable) {
  
  if (updateTable==1) {
    updateTableData = workshopData;
  }
  else if (updateTable==2) {
    updateTableData = adminData;
  }
  else{

  }
  if (document.getElementById("overlayMenu").style.width == "100%") {
    return;
  }
  document.getElementById("2_sideMenuItem").innerText = document.getElementById("2_TempControlData").innerText = updateTableData[indexItemController][2];
  document.getElementById("3_sideMenuItem").innerText = document.getElementById("3_TempControlData").innerText = updateTableData[indexItemController][3] / 2 + "°С";

  for (let i = 0; i < 8; i++) {
    roundUp = updateTableData[i][3] / 2;
    document.getElementById("temp" + i).innerText = roundUp.toFixed() + "°С";
  }

  document.getElementById("4_sideMenuItem").innerText = document.getElementById("4_TempControlData").innerText = updateTableData[indexItemController][4] / 2 + "°С";
  //Режим охлаждения/нагрева
  for (let i = 1; i <= 2; i++) {
    if (updateTableData[indexItemController][5] == i) {
      document.getElementById(i + "Mod").checked = document.getElementById(i + "ModFull").checked = "checked";
    }
  }
  //Режим работы вентиляторов
  for (let i = 0; i <= 3; i++) {
    if (updateTableData[indexItemController][6] == i) {
      document.getElementById(i + "Fan").checked = document.getElementById(i + "FanFull").checked = "checked";
    }
  }
  //заполняем оставшиеся пункты побробного меню
  //Блокировка режимов
  for (let i = 0; i <= 2; i++) {
    if (updateTableData[indexItemController][7] == i) {
      document.getElementById(i + "SysModFull").checked = "checked";
    }
  }

  document.getElementById("8_TempControlData").innerText = (updateTableData[indexItemController][8] - 16) / 2 + "°С";
  document.getElementById("9_TempControlData").innerText = updateTableData[indexItemController][9] / 2 + "°С";
  document.getElementById("0_TempControlInput").max = document.getElementById("1_TempControlInput").max = updateTableData[indexItemController][9];
  document.getElementById("10_TempControlData").innerText = updateTableData[indexItemController][10] / 2 + "°С";
  document.getElementById("0_TempControlInput").min = document.getElementById("1_TempControlInput").min = updateTableData[indexItemController][10];
  document.getElementById("11_TempControlData").innerText = updateTableData[indexItemController][11] / 2 + "°С";
  document.getElementById("12_TempControlData").innerText = updateTableData[indexItemController][12];
  document.getElementById("13_TempControlData").innerText = updateTableData[indexItemController][13] / 2 + "°С";
  for (let i = 0; i <= 2; i++) {
    if (updateTableData[indexItemController][14] == i) {
      document.getElementById(i + "AntifreezeType").checked = "checked";
    }
  }
  //Включен ли контроллер
  //Массив допустимых значеий
  let sysStatus = [2, 3, 6, 7, 10, 11, 14, 15];
  for (let i = 0; i < sysStatus.length; i++) {
    //Если данные с контроллера равны элементу массива
    if (updateTableData[indexItemController][15] == sysStatus[i]) {
      document.getElementById("6SysStatFull").checked = "checked";
      //Разрываем цикл, чтобы изменения прошли
      break;
    } else {
      document.getElementById("4SysStatFull").checked = "checked";
    }
  }
  for (let j = 0; j < 8; j++) {
    for (let i = 0; i < sysStatus.length; i++) {
      if (updateTableData[j][15] == sysStatus[i]) {
        document.getElementById("Controller" + j).src = 'img/tempControlOn.png';
        break;
      } else {
        document.getElementById("Controller" + j).src = 'img/tempControl.png';
      }
    }
  }
  //Включен ли режим работы по периодам 
  let periodStatus = [1, 3, 5, 7, 9, 11, 13, 15];
  for (let i = 0; i < periodStatus.length; i++) {
    //Если данные с контроллера равны элементу массива
    if (updateTableData[indexItemController][15] == periodStatus[i]) {
      document.getElementById("1PeriodFull").checked = "checked";
      //Разрываем цикл, чтобы изменения прошли
      break;
    } else {
      document.getElementById("0PeriodFull").checked = "checked";
    }
  }

  document.getElementById("15_TempControlData").innerText = updateTableData[indexItemController][15];
  //Время в минутах и часах
  document.getElementById("17_16_TempControlData").innerText = updateTableData[indexItemController][17] + ":" + updateTableData[indexItemController][16];
  for (let i = 0; i <= 6; i++) {
    if (updateTableData[indexItemController][18] == i) {
      document.getElementById("WeekSelectFull").value = i;
    }
  }
  // Время включения и отключения
  for (let i = 19; i <= 30; i++) {
    TimeConvertingHour = (updateTableData[indexItemController][i]) * 0.25;
    TimeConvertingMinute = TimeConvertingHour % 1;
    TimeConvertingHour = TimeConvertingHour - TimeConvertingMinute;
    TimeConvertingMinute = TimeConvertingMinute * 60;
    if (TimeConvertingMinute < 10) {
      TimeConvertingMinute = "0" + TimeConvertingMinute;
    }
    document.getElementById(i + "_TempControlData").innerText = TimeConvertingHour + ":" + TimeConvertingMinute;
  }

}