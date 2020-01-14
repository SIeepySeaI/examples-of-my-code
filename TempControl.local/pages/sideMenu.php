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