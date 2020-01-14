"use strict";
var eventObject;
var loadedPage;
var SliderItemIndex;

document.addEventListener("DOMContentLoaded", setEvent); 

function setEvent(){
  //сначала находим родительский элемент по Id
  var divName = document.getElementById('tempControlDiv');
  //Затем, в нем ищем все нужные элементы по тегу
  var childImg = divName.getElementsByTagName('img');
  // С помощью цикла всем назначаем событие
  for(var i=0; i<childImg.length; i++) childImg[i].addEventListener("click", MenuCall);
  //Затем, в нем ищем все нужные элементы по тегу
  
}
function MenuCall() {
    document.getElementById("MenuSelecror").classList.toggle("change");
    if(document.getElementById("sideMenu").style.width === "300px"){
      document.getElementById("sideMenu").style.width = "0px";
      document.getElementById("main").style.marginLeft = "0px";
    }
    else{
      document.getElementById("sideMenu").style.width = "300px";
      document.getElementById("main").style.marginLeft = "300px";
    }
    
}
function overlayCall(){
  document.getElementById("overlayMenu").style.width = "100%";
  document.getElementById("MenuSelecror").classList.toggle("change");
  document.getElementById("sideMenu").style.width = "0px";
  document.getElementById("main").style.marginLeft = "0px";
}
function overlayClose(){
  document.getElementById("overlayMenu").style.width = "0%";
}
function OnChangeRange(SliderItem) {
  SliderItemIndex = SliderItem.id.substr(0,1);
  if (SliderItemIndex == 0||SliderItemIndex == 1) {
    document.getElementById(SliderItemIndex + "_InputValue").innerText = SliderItem.value / 2 + "°С";
  }
  else if (SliderItemIndex == 2) {
    document.getElementById(SliderItemIndex + "_InputValue").innerText = SliderItem.value - 16 + "°С";
  }
  else if (SliderItemIndex == 3||SliderItemIndex == 4) {
    document.getElementById(SliderItemIndex + "_InputValue").innerText = SliderItem.value/2 + "°С";
  }
  else if (SliderItemIndex == 5) {
    document.getElementById(SliderItemIndex + "_InputValue").innerText = SliderItem.value + "°С";
  }
}
