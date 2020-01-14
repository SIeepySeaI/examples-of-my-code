<!doctype html>
<html lang="ru">
<head>
<meta charset="UTF-8">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<meta http-equiv="X-UA-Compatible" content="ie=edge">
<link rel="stylesheet" type="text/css" href="css/normalize.css">
<link rel="stylesheet" type="text/css" href="css/bootstrap/bootstrap.min.css">
<link rel="stylesheet" type="text/css" href="css/styleIndex.css">
<link rel="stylesheet" type="text/css" href="css/styleMenu.css">
<script src="js/bootstrap/bootstrap.min.js"></script>
<script src="js/jquery.min.js"></script>
<script src="js/MainPageScript.js"></script>
<script src="js/timerDBRequest.js"></script>
<script src="js/WriteInDB.js"></script>
<title>Общий мониторинг</title>
</head>
<body>
    <header>
    </header>
    <div id="main">
<!-- Боковая менюшка -->
<!---->
<div id="sideMenu" class="sideMenuClass">
	<div class ="sideMenuContent">
		<ul class="Parameters">
			<li>Рабочая зона:<div id="2_sideMenuItem"></div>
			</li>
			<li>Температура в помещении:<div id="3_sideMenuItem"></div>
			</li>
			<li>Поддерживаемая температура:<div id="4_sideMenuItem"></div>
				<div id="0_InputValue"></div>
				<input type="range" class="form-control-range" id="0_TempControlInput" min="20" max="60"
					oninput="OnChangeRange(this)" value="20">
			</li>
			<li>Режим работы:<div id="5_sideMenuItem"></div>
				<div class="form-check">
					<label class="form-check-label">
						<input id="1Mod" name="Mod" type="radio" class="form-check-input" name="optradio">Охлаждение
					</label>
				</div>
				<div class="form-check">
					<label class="form-check-label">
						<input id="2Mod" name="Mod" type="radio" class="form-check-input" name="optradio">Нагрев
					</label>
				</div>
			</li>
			<li>Скорость вентиляторов:<div id="6_sideMenuItem"></div>
				<div class="form-check">
					<label class="form-check-label">
						<input id="0Fan" name="Fan" type="radio" class="form-check-input" name="optradio">Авто
					</label>
				</div>
				<div class="form-check">
					<label class="form-check-label">
						<input id="1Fan" name="Fan" type="radio" class="form-check-input" name="optradio">Низкая
					</label>
				</div>
				<div class="form-check">
					<label class="form-check-label">
						<input id="2Fan" name="Fan" type="radio" class="form-check-input" name="optradio">Средняя
					</label>
				</div>
				<div class="form-check">
					<label class="form-check-label">
						<input id="3Fan" name="Fan" type="radio" class="form-check-input" name="optradio">Максимальная
					</label>
				</div>
			</li>
		</ul>
		<button id="sideSaveBtn" type="button" class="btn btn-primary" onclick="SaveParam()">Сохранить</button>
		<button id="overlayOpenBtn" type="button" class="btn btn-secondary" onclick="overlayCall()">Подробнее</button>
	</div>
</div>
<!---->
<!---->
        <div id="MenuSelecror" onclick="MenuCall()">
            <div id="MenuPrimitive1"></div>
            <div id="MenuPrimitive2"></div>
            <div id="MenuPrimitive3"></div>
        </div>
<!-- Подробные параметры -->
        <?php include ('./pages/FullOptions.php') ?>
<!-- Элементы контроля -->
<div class="grid">
        
<div id="tempControlDiv">
        <?php
        define("tempControlItem",8,false);
        for($i=0;$i<tempControlItem;$i++):
        ?>
        <div class="div<?php echo($i) ?>" onclick='clickController(this)'>
        <img id="Controller<?php echo($i) ?>" class="tempControl item<?php echo($i) ?>"  src="img\tempControl.png" alt="пикча" >
        <p id="temp<?php echo($i) ?>" class="temperature">44°C</p>
        </div>
        <?php
        endfor;
        ?>
            
    </div>
<div id="fanDiv">
        <?php
        define("fanItem",40,false);
        for($i=1;$i<=fanItem;$i++):
        ?>
        <img class="fan item<?php echo($i) ?>" src="img\VolcanoFan.png" alt="пикча" >
        <?php
        endfor;
        ?>
</div>
  </div>      
    </div>
        <footer></footer>

</body>
</html>