<div id="overlayMenu" class="overlay">
	<a href="javascript:void(0)" class="closebtn" onclick="overlayClose()">&times;</a>
	<div class="overlay-content">

		<div class="flex-container">
			<div class="flex-item">
				<ul class="ParamBlock">
					<li id="Headline">Температурные показатели</li>
					<li>Температура в помещении:<div id="3_TempControlData" class="TempCels"></div>
					</li>
					<li>Поддерживаемая температура:<div id="4_TempControlData" class="ControllerValue TempCels"></div>
						<div id="1_InputValue" class="ControllerValue TempCels"></div>
						<input type="range" class="form-control-range" id="1_TempControlInput" min="20" max="60"
							oninput="OnChangeRange(this)" value="20">
					</li>
					<li>Калибровка температурного датчика:<div id="8_TempControlData" class="ControllerValue TempCels"></div>
						<div id="2_InputValue" class="ControllerValue TempCels"></div>
						<input type="range" class="form-control-range" id="2_TempControlInput" min="0" max="32"
							oninput="OnChangeRange(this)" value="0">
					</li>
					<li>Максимальная температура:<div id="9_TempControlData" class="ControllerValue TempCels"></div>
						<div id="3_InputValue" class="ControllerValue TempCels"></div>
						<input type="range" class="form-control-range" id="3_TempControlInput" min="10" max="100"
							oninput="OnChangeRange(this)" value="10">
					</li>
					<li>Минимальная температура:<div id="10_TempControlData" class="ControllerValue TempCels"></div>
						<div id="4_InputValue" class="ControllerValue TempCels"></div>
						<input type="range" class="form-control-range" id="4_TempControlInput" min="10" max="100"
							oninput="OnChangeRange(this)" value="10">
					</li>
					<li>Противозамораживающая температура:<div id="13_TempControlData" class="ControllerValue TempCels"></div>
						<div id="5_InputValue" class="ControllerValue TempCels"></div>
						<input type="range" class="form-control-range" id="5_TempControlInput" min="4" max="44"
							oninput="OnChangeRange(this)" value="4">
					</li>
				</ul>
			</div>
			<div class="flex-item">
				<ul class="ParamBlock">
					<li id="Headline">Режимы работы</li>

					<li>Режим активного поддержания температуры:<div id="5_TempControlData"></div>
						<div class="form-check">
							<label class="form-check-label">
								<input id="6SysStatFull" name="SysStatFull" type="radio" class="form-check-input"
									name="optradio">Включен
							</label>
						</div>
						<div class="form-check">
							<label class="form-check-label">
								<input id="4SysStatFull" name="SysStatFull" type="radio" class="form-check-input"
									name="optradio">Выключен
							</label>
						</div>
					</li>

					<li>Режим работы:<div id="5_TempControlData"></div>
						<div class="form-check">
							<label class="form-check-label">
								<input id="1ModFull" name="ModFull" type="radio" class="form-check-input" name="optradio">Охлаждение
							</label>
						</div>
						<div class="form-check">
							<label class="form-check-label">
								<input id="2ModFull" name="ModFull" type="radio" class="form-check-input" name="optradio">Нагрев
							</label>
						</div>
					</li>
					<li>Блокировка режимов:<div id="7_TempControlData"></div>
						<div class="form-check">
							<label class="form-check-label">
								<input id="0SysModFull" name="SysModFull" type="radio" class="form-check-input" name="optradio">По
								умолчанию
							</label>
						</div>
						<div class="form-check">
							<label class="form-check-label">
								<input id="1SysModFull" name="SysModFull" type="radio" class="form-check-input" name="optradio">Только
								охлаждение
							</label>
						</div>
						<div class="form-check">
							<label class="form-check-label">
								<input id="2SysModFull" name="SysModFull" type="radio" class="form-check-input" name="optradio">Только
								нагрев
							</label>
						</div>
					</li>
					<li>Скорость вентиляторов:<div id="6_TempControlData"></div>
						<div class="form-check">
							<label class="form-check-label">
								<input id="0FanFull" name="FanFull" type="radio" class="form-check-input" name="optradio">Авто
							</label>
						</div>
						<div class="form-check">
							<label class="form-check-label">
								<input id="1FanFull" name="FanFull" type="radio" class="form-check-input" name="optradio">Низкая
							</label>
						</div>
						<div class="form-check">
							<label class="form-check-label">
								<input id="2FanFull" name="FanFull" type="radio" class="form-check-input" name="optradio">Средняя
							</label>
						</div>
						<div class="form-check">
							<label class="form-check-label">
								<input id="3FanFull" name="FanFull" type="radio" class="form-check-input" name="optradio">Максимальная
							</label>
						</div>
					</li>
					<li>Противозамораживающий режим работы:<div id="14_TempControlData"></div>
						<div class="form-check">
							<label class="form-check-label">
								<input id="0AntifreezeType" name="AntifreezeTypeFull" type="radio" class="form-check-input"
									name="optradio">Клапан закрыт
							</label>
						</div>
						<div class="form-check">
							<label class="form-check-label">
								<input id="1AntifreezeType" name="AntifreezeTypeFull" type="radio" class="form-check-input"
									name="optradio">Клапан открыт
							</label>
						</div>
						<div class="form-check">
							<label class="form-check-label">
								<input id="2AntifreezeType" name="AntifreezeTypeFull" type="radio" class="form-check-input"
									name="optradio">Клапан открыт и работают вентиляторы
							</label>
						</div>
					</li>
				</ul>

			</div>
			<div class="flex-item">

				<ul class="ParamBlock">
					<li id="Headline">Настройки времени и продолжительности работы в рабочую неделю</li>
					<li>Время (в часах/минутах):
						<div id="17_16_TempControlData" class="ControllerValue TimeOldValue"></div>
						<input id="17_16_TimeInput" type="time" class="form-control timeSelect">
					</li>
					<li>День недели:<div id="18_TempControlData"></div>
						<form class="form-inline">
							<label class="my-1 mr-2" for="WeekSelectFull">День недели</label>
							<select class="custom-select my-1 mr-sm-2" id="WeekSelectFull">
								<option value="0">Понедельник</option>
								<option value="1">Вторник</option>
								<option value="2">Среда</option>
								<option value="3">Четверг</option>
								<option value="4">Пятница</option>
								<option value="5">Суббота</option>
								<option value="6">Воскресенье</option>
							</select>
					</li>

					<li>Режим работы по периодам:<div id="5_TempControlData"></div>
						<div class="form-check">
							<label class="form-check-label">
								<input id="1PeriodFull" name="PeriodFull" type="radio" class="form-check-input" name="optradio">Включен
							</label>
						</div>
						<div class="form-check">
							<label class="form-check-label">
								<input id="0PeriodFull" name="PeriodFull" type="radio" class="form-check-input" name="optradio">Отключен
							</label>
						</div>
					</li>

					<li>
						<table class="workTime">
							<td>
								Период 1 <br />
								Влючение
								<div id="19_TempControlData" class="ControllerValue TimeOldValue"></div>
								<input id="19_TimeInput" type="time" class="form-control timeSelect" step="900">
								Отключение
								<div id="20_TempControlData" class="ControllerValue TimeOldValue"></div>
								<input id="20_TimeInput" type="time" class="form-control timeSelect" step="900">

							</td>
							<td>
								Период 2 <br />
								Влючение
								<div id="21_TempControlData" class="ControllerValue TimeOldValue"></div>
								<input id="21_TimeInput" type="time" class="form-control timeSelect" step="900">
								Отключение
								<div id="22_TempControlData" class="ControllerValue TimeOldValue"></div>
								<input id="22_TimeInput" type="time" class="form-control timeSelect" step="900">
							</td>
						</table>
					</li>
				</ul>
			</div>

			<div class="flex-item">
				<ul class="ParamBlock">
					<li id="Headline">Настройки включения в выходные</li>
					<li>
						Время включения/отключения в субботу: <br />
						<table class="workTime">
							<td>
								Период 1 <br />
								Влючение <br />
								<div id="23_TempControlData" class="ControllerValue TimeOldValue"></div>
								<input id="23_TimeInput" type="time" class="form-control timeSelect" step="900">
								Отключение <br />
								<div id="24_TempControlData" class="ControllerValue TimeOldValue"></div>
								<input id="24_TimeInput" type="time" class="form-control timeSelect" step="900">
							</td>
							<td>
								Период 2 <br />
								Влючение <br />
								<div id="25_TempControlData" class="ControllerValue TimeOldValue"></div>
								<input id="25_TimeInput" type="time" class="form-control timeSelect" step="900">
								Отключение <br />
								<div id="26_TempControlData" class="ControllerValue TimeOldValue"></div>
								<input id="26_TimeInput" type="time" class="form-control timeSelect" step="900">
							</td>
						</table>
					</li>
					<li>
						Время включения/отключения в воскресенье:<br />
						<table class="workTime">
							<td>
								Период 1 <br />
								Влючение <br />
								<div id="27_TempControlData" class="ControllerValue TimeOldValue"></div>
								<input id="27_TimeInput" type="time" class="form-control timeSelect" step="900">
								Отключение <br />
								<div id="28_TempControlData" class="ControllerValue TimeOldValue"></div>
								<input id="28_TimeInput" type="time" class="form-control timeSelect" step="900">
							</td>
							<td>
								Период 2 <br />
								Влючение <br />
								<div id="29_TempControlData" class="ControllerValue TimeOldValue"></div>
								<input id="29_TimeInput" type="time" class="form-control timeSelect" step="900">
								Отключение <br />
								<div id="30_TempControlData" class="ControllerValue TimeOldValue"></div>
								<input id="30_TimeInput" type="time" class="form-control timeSelect" step="900">
							</td>
						</table>
					</li>
				</ul>
			</div>

			<div class="flex-item">
				<ul class="ParamBlock">
					<li id="Headline">Техническая информация</li>
					<li>Обслуживаемая зона:<div id="2_TempControlData"></div>
					</li>
					<li>Значение вольтажа выходного сигнала(0-10V):<div id="12_TempControlData"></div>
					</li>
					<li>Модуляция значения выходного сигнала<br>(1-4V, 0 - возврат к заводским настройкам):<div
							id="11_TempControlData"></div>
					</li>
					<li>Статус контроллера(On/Off):<div id="15_TempControlData"></div>
					</li>
				</ul>
				<button id="LoadButton" type="button" class="btn btn-primary" onclick="SaveParam()">Сохранить</button>
				<button type="button" class="btn btn-danger" data-dismiss="modal" onclick="overlayClose()">Отмена</button>
			</div>
		</div>
	</div>
</div>