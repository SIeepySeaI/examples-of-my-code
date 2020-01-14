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
        <?php
        define("tempControlItem",8,false);
        for ($i=0; $i < tempControlItem; $i++):
        ?>
        <div id="border<?php echo($i)?>" class="borderZone"></div>
        <?php endfor; ?>
        <!-- Боковая менюшка -->
        <?php include ('./pages/sidemenu.php') ?>
        <div id="MenuSelecror" onclick="MenuCall()">
            <div id="MenuPrimitive1"></div>
            <div id="MenuPrimitive2"></div>
            <div id="MenuPrimitive3"></div>
        </div>
        <!-- Кнопка переключения между зданиями -->
        <div class="SelectMap">
            <div></div>
            <input type="checkbox" id="MapCheckbox" onclick='ChangeMap(this)'/>
            <label id="selecterLabel" for="MapCheckbox"></label>
        </div>
        <!-- Подробные параметры -->
        <?php include ('./pages/FullOptions.php') ?>
        <!-- Элементы контроля -->
        <div id="Maps">
            <div class="Map" id="Workshop">

                <div id="tempControlDiv">
                    <?php for($i=0;$i<tempControlItem;$i++): ?>
                    <div class="div<?php echo($i) ?>" onclick='clickController(this)'>
                        <img id="Controller<?php echo($i) ?>" class="tempControl item<?php echo($i) ?>"
                            src="img\tempControl.png" alt="пикча">
                        <p id="temp<?php echo($i) ?>" class="temperature">44°C</p>
                    </div>
                    <?php endfor; ?>
                </div>
                <div id="fanDiv">
                    <?php define("fanItem",40,false); for($i=1;$i<=fanItem;$i++): ?>
                    <img class="fan item<?php echo($i) ?>" src="img\VolcanoFan.png" alt="пикча">
                    <?php endfor; ?>
                </div>
            </div>
            <?php include ('./pages/AdminBilding.php') ?>
        </div>
    </div>
    <footer></footer>
</body>

</html>