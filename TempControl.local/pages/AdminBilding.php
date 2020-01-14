<div class="Map" id="Administration">
  <?php define("floorItem",3,false); for($i=0;$i<floorItem;$i++): ?>

  <div class="Floor" id="floor<?php echo($i) ?>" onclick ="clickController(this)">
  <?php echo($i) + 1 ?>-й Этаж
    <div class="floorTemperature" id="floorTemp<?php echo($i) ?>">
      44°С
    </div>
  </div>
  <?php endfor; ?>
</div>