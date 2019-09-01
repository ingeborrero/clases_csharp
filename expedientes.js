/*
Developer: Jorge Armando Borrero Gomez
Date: Abril 2018
Description: Funciones usadas para consulta de un expediente en particular usando las librerias  como acordeones, modales, y visores..

*/
var g_BuscardorExpediente;
	helpPlaceHolder = ["Ej: HII09C", "Ej: 91519643", "Ej:20154", "Eje:2017-123456"];
var	g_abiertos 		= false
var g_cerrados 		= false
var g_fiscal 		= false;
var g_coactivo 		= false;
var g_exp_abiertos 	= 0;
var g_exp_cerrados 	= 0;
var g_exp_fiscal 	= 0;
var g_exp_coactivo 	= 0;


function pruebas() {
	crearCargando();
	$.ajax({
		url: dominio_PRODUCTO + 'Paginas/Pruebas.aspx',
		cache: false,
		type: 'POST',
		dataType: 'json',
		data: {
			develop: 'JBorrero'
		},
		xhrFields: {
			withCredentials: true
		}
	}).done(function (result) {
		eliminarCargando();
		if (result.codigoError === "0") {
            //crearModalECO(result.data[0]);
			//crearZonasECO(result.data[1]);
			//crearAcordeonECO(result.data[2]);
			//crearComboECO(result.data[1]);
			ejecutarRespuestaAJAX(result);
        } else {
            new AlertLM().show(document.body, result.mensajeError);
        }
	}).fail(function (jqXHR, textStatus) {
		eliminarCargando();
		alertLM.show(document.body, "Se han presentado inconvenientes en la aplicación. <br>Reporte error a la central.");
	});

	// crearCargando();
	// $.ajax({
    //        url: dominio_PRODUCTO + "Paginas/Pruebas.aspx",
    //        dataType: "json",
    //        type: "POST",
    //        success: function (r) {
    //            eliminarCargando();
	//
    //  		  if (r.codigoError === "0") {
    //                crearModalECO(r.data[0]);
	// 			   crearZonasECO(r.data[1]);
    //            } else {
    //                new AlertLM().show(document.body, r.mensajeError);
    //            }
    //        },
    //        error: function (e) {
    //            eliminarCargando();
    //            alertLM.show('body', 'Error en consulta: ' + e);
    //        }
    //    });
}


function iniciarConsultaExpediente(){
	let divcontenedor = document.createElement('div');
	 	divcontenedor.setAttribute('class', 'boxModalConsulta');
	 	$('body').append(divcontenedor);

		g_BuscardorExpediente = undefined;
		if(!g_BuscardorExpediente) {
			g_BuscardorExpediente = new crearBuscadorExp();
		}
		crear_containViewer_ECO('CONSULTA DE EXPEDIENTES', 'cvconsulta', g_BuscardorExpediente.mainFrame,'','');
		cargarTipoFiltros();
}

function cargarTipoFiltros(){
	var options = ["Placa", "Documento de Identidad", "Id Expediente", "Año - Nro Expediente"];
	var id_cliente = $('#ddl_Proyecto').val();

	if(id_cliente == 1 || id_cliente == 2 || id_cliente == 10) { //Vehiculos
		options = ["Placa", "Documento de Identidad", "Id Expediente", "Año - Nro Expediente"]
		helpPlaceHolder = ["Ej: GKA46D", "Ej: 96352421", "Ej: 6693987", "Ej: 2012-8050"];
	}else if (id_cliente == 7) {//Estampillas
		options = ["Nit", "Id Expediente", "Nombre de la entidad", "Año - Nro Expediente"]
		helpPlaceHolder = ["Ej: 890205229", "Ej:20154", "Ej: ALCALDIA DE MALAGA", "Ej: 2017-123456"];
	}else if (id_cliente == 16) {//Resgistro
		options = ["Boleta", "Documento de Identidad", "Id Expediente", "Año - Nro Expediente"]
		helpPlaceHolder = ["Ej: 680047877", "Ej: 91519643", "Ej: 30154", "Ej: 2017-123456"];
	}else {//Otros
		options = ["Documento de Identidad", "Id Expediente"]
		helpPlaceHolder = ["Ej: 91519643", "Ej: 30154"];
	}

	$('#ddl_BuscarPor').empty();
	$.each(options, function(i, p) {
    	$('#ddl_BuscarPor').append($('<option></option>').val(p).html(p));
	});
	cargarCamposBuscar();
}


function cargarCamposBuscar() {
	var textofiltro = $('#ddl_BuscarPor').val();
	var indexSelec = $("#ddl_BuscarPor").prop('selectedIndex');
	var helpPlace = helpPlaceHolder[indexSelec];
	$('#campoTit1').text(textofiltro);
	$('#txtCampo1').attr("placeholder", helpPlace);
}

function obtenerTipoFiltro(_idCliente, _textoFiltro) {
	var filtroRet;
	switch(_textoFiltro.toUpperCase()){
		case 'PLACA':
			filtroRet = 'PLACA';
			break;
		case 'ID EXPEDIENTE':
			filtroRet = 'EXPEDIENTE';
			break;
		case 'NIT':
		case 'DOCUMENTO DE IDENTIDAD':
			filtroRet = 'DOCUMENTO';
			break;
		case 'BOLETA':
			filtroRet = 'BOLETA';
			break;
		case 'AÑO - NRO EXPEDIENTE':
			filtroRet = 'ANIO-NRO_EXPEDIENTE';
			break;
		default:
			filtroRet = undefined;
	}

	return filtroRet;
}



function buscarExpediente(_desdefiltros, _control) {
	crearCargando();
	var idCliente = $('#ddl_Proyecto').val();
	var textcliente = $('#ddl_Proyecto option:selected').html();
	var textofiltro = $('#ddl_BuscarPor').val();
	var valorbusqueda = $('#txtCampo1').val();
	var filtro = obtenerTipoFiltro(idCliente, textofiltro);

	// var abiertos = false;
	// var solofiscal = false;
	// var solocoactivo = false;

	if(_desdefiltros == undefined) {
		// $(".boxCheckAbierto" ).removeClass('active');
		// $('#chkFilAbiertos')[0].checked = false;
		// $('#chkFilFiscal')[0].checked = false;
		// $('#chkFilCoactivo')[0].checked = false;

		//Dejo activos todos los filtros menos los cerrados
		g_abiertos = true;
		g_cerrados = false;
		g_fiscal = true;
		g_coactivo = true;
		// $('#chkFilAbiertos')[0].checked = true;
	} else {
		if ($('#chkFilAbiertos')[0] != undefined) {
			g_abiertos = $('#chkFilAbiertos')[0].checked;
		}
		if ($('#chkFilCerrados')[0] != undefined) {
			g_cerrados = $('#chkFilCerrados')[0].checked;
		}
		if ($('#chkFilFiscal')[0] != undefined) {
			g_fiscal = $('#chkFilFiscal')[0].checked;
		}
		if ($('#chkFilCoactivo')[0] != undefined) {
			g_coactivo = $('#chkFilCoactivo')[0].checked;
		}
	}

	// if(abiertos==true || solofiscal==true || solocoactivo==true){
	// 	$(".boxCheckAbierto" ).addClass('active');
	// }
	// else {
	// 	$(".boxCheckAbierto" ).removeClass('active');
	// }

	//alert('g_abiertos:'+g_abiertos +' | g_cerrados:'+ g_cerrados +' | g_fiscal:'+ g_fiscal +' | g_coactivo:'+ g_coactivo);

	$.ajax({
	    url: dominio_PRODUCTO + 'Paginas/Expediente/datosConsultaExp.aspx',
	    cache: false,
	    type: 'POST',
	    dataType: 'text',
	    data: {
			idCliente: idCliente,
			filtro : filtro,
			valor: valorbusqueda,
			tipo: 1,
			abiertos: g_abiertos,
			cerrados: g_cerrados,
			fiscal: g_fiscal,
			coactivo: g_coactivo
	    },
	    xhrFields: {
	        withCredentials: true
	    }
	}).done(function (resultado) {
	     eliminarCargando();
		 //<JBorrero 20-02-2018> Controlar la salida cuando se borra la session
		 if(resultado.indexOf("window.top.loca") >= 0) {
			 $('body').append(resultado);
			 return;
		 }

		 $('#btnFiltrarExp').css('display','none');
		 var cadena = JSON.parse(resultado);
		 var num_registros =  cadena[0].num_registros;

		 if (_desdefiltros == undefined) {
			 g_exp_abiertos  	= cadena[0].exp_abiertos;
			 g_exp_cerrados  	= cadena[0].exp_cerrados;
			 g_exp_fiscal  		= cadena[0].exp_fiscal;;
			 g_exp_coactivo  	= cadena[0].exp_coactivo;
	 	 }

		 if (_desdefiltros == undefined  &  g_exp_abiertos == 0) {
		 	//$('#chkFilAbiertos')[0].checked = false;
			g_abiertos = true;
			g_cerrados = true;
			g_fiscal = true;
			g_coactivo = true;
		 }

		if(num_registros > 0) {
			//Cargo el acordeon con los datos
			$('#zonaBoxPrincipal').empty();
			crearCargando();
			$.ajax({
				url: dominio_PRODUCTO + 'Paginas/Expediente/datosConsultaExp.aspx',
				cache: false,
				type: 'POST',
				dataType: 'json',
				data: {
					idCliente: idCliente,
					filtro : filtro,
					valor: valorbusqueda,
					tipo: 2,
					abiertos: g_abiertos,
					cerrados: g_cerrados,
					fiscal: g_fiscal,
					coactivo: g_coactivo
				},
				xhrFields: {
					withCredentials: true
				}
			}).done(function (result) {
				eliminarCargando();
				//var jsonResult = JSON.parse(resultadoJson);
				if (result.codigoError === "0") {
					ejecutarRespuestaAJAX(result);
					$('.clsboxModal').remove();
					if ((g_exp_abiertos>0 & g_exp_cerrados>0) || (g_exp_fiscal>0 & g_exp_coactivo>0)) {
						$('#btnFiltrarExp').css('display','block');
					}
					//Ejecutar evento de clic en primer registro
					$('.boxResult:first .boxTitleHeader:first').trigger('click');
					$('.boxControlEstados').css('display','block');
				} else {
					alertLM.show(document.body, result.mensajeError);
				}
			}).fail(function (jqXHR, textStatus) {
				eliminarCargando();
				alertLM.show(document.body, "Se han presentado inconvenientes en la aplicación. <br>Reporte error a la central.");
			});

		}else {
			if(_desdefiltros == undefined) {
				$('#zonaBoxPrincipal').empty();
				$('#zonaBoxPrincipal').append('<div class="alert alert-danger"><strong>Expediente no encontrado! </strong>No se encontró información para el expediente consultado.</div>');
			}
			else {
				if(_control != undefined || _control != null) {
					$(_control)[0].checked = false;
				}
				alertLM.show(document.body, "No existen expedientes con los filtros aplicados.");
			}
		}
	}).fail(function (jqXHR, textStatus) {
	    eliminarCargando();
	    alertLM.show(document.body, "Se han presentado inconvenientes en la aplicación. <br>Reporte error a la central.");
	});
}


function modalFiltrosExpediente(_me) {
	var html_abierto 	= '';
	var html_cerrado 	= '';
	var html_fiscal 	= '';
	var html_coactivo 	= '';

	crearCargando();
	$.ajax({
		url: dominio_PRODUCTO + 'Paginas/Expediente/armarFiltroExp.aspx',
		cache: false,
		type: 'POST',
		dataType: 'json',
		data: {
			developer: 'JBorrero',
			offl: parseInt($(_me).offset().left) + $(_me).height(),
			offt: parseInt($(_me).offset().top) + $(_me).width()
		},
		xhrFields: {
			withCredentials: true
		}
	}).done(function (result) {
		eliminarCargando();
		if (result.codigoError === "0") {
			ejecutarRespuestaAJAX(result);
			// if(g_exp_abiertos > 0 ) {
			// 	//onchange="buscarExpediente(true, this)">
			//  	html_abierto =`<div class="custom-control custom-checkbox">
			// 				  <input type="checkbox" class="custom-control-input" id="chkFilAbiertos" value="abiertos" onchange="">
			// 				  <label class="custom-control-label" for="chkFilAbiertos">Abiertos</label>
			// 				  </div>`;
			// }
			// if(g_exp_cerrados > 0) {
			//  	html_cerrado =`<div class="custom-control custom-checkbox">
			// 				  <input type="checkbox" class="custom-control-input" id="chkFilCerrados" value="cerrados" onchange="">
			// 				  <label class="custom-control-label" for="chkFilCerrados">Cerrados</label>
			// 				  </div>`;
			// }
			// if(g_exp_fiscal > 0) {
			// 	html_fiscal =`<div class="custom-control custom-checkbox">
			// 				  <input type="checkbox" class="custom-control-input" id="chkFilFiscal" value="fiscal" onchange="">
			// 				  <label class="custom-control-label" for="chkFilFiscal">En Fiscalización</label>
			// 				</div>`;
			// }
			// if(g_exp_coactivo > 0) {
			// 	html_coactivo =`<div class="custom-control custom-checkbox">
			// 				  <input type="checkbox" class="custom-control-input" id="chkFilCoactivo" value="coactivo" onchange="">
			// 				  <label class="custom-control-label" for="chkFilCoactivo">En Coactivo</label>
			// 				</div>`;
			// }

			if (g_exp_abiertos > 0 & g_exp_cerrados > 0) {
				//onchange="buscarExpediente(true, this)">
			 	html_abierto =`<div class="custom-control custom-checkbox">
							  <input type="checkbox" class="custom-control-input" id="chkFilAbiertos" value="abiertos" onchange="">
							  <label class="custom-control-label" for="chkFilAbiertos">Abiertos</label>
							  </div>`;
			 	html_cerrado =`<div class="custom-control custom-checkbox">
							  <input type="checkbox" class="custom-control-input" id="chkFilCerrados" value="cerrados" onchange="">
							  <label class="custom-control-label" for="chkFilCerrados">Cerrados</label>
							  </div>`;
			}

			if (g_exp_fiscal > 0 & g_exp_coactivo > 0) {
				html_fiscal =`<div class="custom-control custom-checkbox">
							  <input type="checkbox" class="custom-control-input" id="chkFilFiscal" value="fiscal" onchange="">
							  <label class="custom-control-label" for="chkFilFiscal">En Fiscalización</label>
							</div>`;
				html_coactivo =`<div class="custom-control custom-checkbox">
							  <input type="checkbox" class="custom-control-input" id="chkFilCoactivo" value="coactivo" onchange="">
							  <label class="custom-control-label" for="chkFilCoactivo">En Coactivo</label>
							</div>`;
			}


			var html_filtros = '<h5 class="modal-title">Filtrar expedientes</h5>'+html_abierto+ html_cerrado+html_fiscal+html_coactivo;

			$('#zonaListaFiltros').append(html_filtros);
			 //Seteo los check Box con los ultimos estados de filtros
			if( $('#chkFilAbiertos')[0] != undefined )	{		$('#chkFilAbiertos')[0].checked = g_abiertos; }
			if( $('#chkFilCerrados')[0] != undefined )	{		$('#chkFilCerrados')[0].checked = g_cerrados; }
			if( $('#chkFilFiscal')[0] != undefined )	{		$('#chkFilFiscal')[0].checked = g_fiscal; }
			if( $('#chkFilCoactivo')[0] != undefined )	{		$('#chkFilCoactivo')[0].checked = g_coactivo; }

		} else {
			alertLM.show(document.body, result.mensajeError);
		}
	}).fail(function (jqXHR, textStatus) {
		eliminarCargando();
		alertLM.show(document.body, "Se han presentado inconvenientes en la aplicación. <br>Reporte error a la central.");
	});
}

function cerarModalVerExpediente() {
	var modalConsulta;
	var divcontenedor = document.createElement('div');
	divcontenedor.setAttribute('class', 'boxModalWindow');
	var boxTituloWindow = document.createElement('div');
	boxTituloWindow.setAttribute('class', 'boxTituloWindow');
	boxTituloWindow.innerHTML = '<b>Aca iria titulo</b>  - ';
	var boxCuerpo = document.createElement('div');
	boxCuerpo.setAttribute('class', 'boxCuerpo');
	boxCuerpo.setAttribute('id', 'boxCuerpo');
	var resAcordeonBusqueda = document.createElement('div');
	resAcordeonBusqueda.setAttribute('id', 'resAcordeonBusqueda');
	resAcordeonBusqueda.setAttribute('class', 'resAcordeonBusqueda');
	$(resAcordeonBusqueda).append(boxTituloWindow);
	var resAcordeonDetalle = document.createElement('div');
	resAcordeonDetalle.setAttribute('id', 'resAcordeonDetalle');
	resAcordeonDetalle.setAttribute('class', 'resAcordeonDetalle');
	$(boxCuerpo).append(resAcordeonBusqueda);
	$(boxCuerpo).append(resAcordeonDetalle);

	$('body').append(divcontenedor);

	 if (modalConsulta) {
			 modalConsulta.destroy();
		 }

	 modalConsulta = new ModalViewer({
		 hostDiv: divcontenedor,
		 widthV: '97%',
		 heightV: '98%',
		 topV: '1%',
		 leftV: '1%',
		 bgColor: '#FFF',
		 addClassViewer: 'boxModalDetalle',
		 draggable: false,
		 scrollSyc: true,
		 clickPreClose: function () {
		 },
		 clickClose: function () {
			  $('.boxModalWindow').remove();
		 }
	 });
	 modalConsulta.addContent(boxCuerpo);
}


function verDetalleExpediente(_idCliente, _idExpediente, _textoGuia) {
	crearCargando();
	$.ajax({
		url: dominio_PRODUCTO + 'Paginas/Expediente/detalleExpediente.aspx',
		cache: false,
		type: 'POST',
		dataType: 'text',
		data: {
			idCliente: _idCliente,
			idExpediente : _idExpediente,
			textoguia: _textoGuia
		},
		xhrFields: {
			withCredentials: true
		}
	}).done(function (result) {
		eliminarCargando();
		//<JBorrero 20-02-2018> Controlar la salida cuando se borra la session
		if(result.indexOf("window.top.loca") >= 0) {
			$('body').append(result);
			return;
		}
		result = JSON.parse(result);
		$('#zonaBoxDerAcordeon').empty();
		if (result.codigoError === "0") {
			ejecutarRespuestaAJAX(result);
			$('#zonaboxTituloDetalle').text(_textoGuia);
		} else {
			alertLM.show(document.body, result.mensajeError);
		}
		// var jsonResult = JSON.parse(resultadoJson);
		// crearAcordeonECO(jsonResult);
		// $('.boxTitleHeader').removeClass('active');
		// $('#deta_expediente_' + _idExpediente +  ' .boxTitleHeader').addClass('active');
	}).fail(function (jqXHR, textStatus) {
		eliminarCargando();
		alertLM.show(document.body, "Se han presentado inconvenientes en la aplicación. <br>Reporte error a la central.");
	});
}

function crearBuscadorExp() {
	var boxBuscardorExp,
		boxControles,
		boxControl,
		labelTitulo,
		cboProyecto,
		cboFiltros,
		inputValor,
		btnBuscarExp,
		btnEstados,
		resAcordeonBusqueda;

	this.mainFrame = document.createElement('div');
	this.mainFrame.setAttribute('class', 'boxConsultaExp');

	boxBuscardorExp = document.createElement('div');
	boxBuscardorExp.setAttribute('class', 'boxBuscardorExp');
	$(this.mainFrame).append(boxBuscardorExp);

	boxControles = document.createElement('div');
	boxControles.setAttribute('class', 'boxControles');
	$(boxBuscardorExp).append(boxControles);

	//CheckBox Fitra Estado Abiertos/Todos
	 boxControl = document.createElement('div');
	 boxControl.setAttribute('id', 'boxControlEstados');
	 boxControl.setAttribute('class', 'boxControlEstados');
	 $(boxControl).css('display', 'none');
	 boxControl.innerHTML = 'Filtros adicionales sobre los expedientes';
	 btnEstados = document.createElement('input');
	 $(boxBuscardorExp).append(boxControl);

	//Div con el boton de filtro adicional de abiertos/todos
	var boxCheckAbierto = document.createElement('div');
	//boxCheckAbierto.setAttribute('id','boxCheckAbierto');
	boxCheckAbierto.setAttribute('id','boxFiltrosAplica');
	boxCheckAbierto.setAttribute('class','boxFiltrosAplica');
	//boxCheckAbierto.setAttribute('class','boxCheckAbierto glyphicon glyphicon-filter');
	boxCheckAbierto.innerHTML = '<div class="btn-group"><button type="button" class="boxCheckAbierto glyphicon glyphicon-filter" data-toggle="dropdown"><span class="caret"></span></button><ul class="dropdown-menu" role="menu"><li><input type="checkbox" id="chkFilAbiertos" name="chkFilAbiertos" value="abiertos" onchange="buscarExpediente(true, this)"> Solo abiertos</li><li><input type="checkbox" id="chkFilFiscal" name="chkFilFiscal" value="fiscal" onchange="buscarExpediente(true,this)"> En fiscalización</li><li><input type="checkbox" id="chkFilCoactivo" name="chkFilCoactivo" value="coactivo" onchange="buscarExpediente(true,this)"> En coactivo</li><li class="divider"></li></ul></div>';
	$(boxControl).append(boxCheckAbierto);
	$(boxBuscardorExp).append(boxControl);


	resAcordeonBusqueda = document.createElement('div');
	resAcordeonBusqueda.setAttribute('class','resAcordeonBusqueda');
	resAcordeonBusqueda.setAttribute('id','resAcordeonBusqueda');
	$(boxBuscardorExp).append(resAcordeonBusqueda);

	//Proyecto
	boxControl = document.createElement('div');
	boxControl.setAttribute('class', 'boxControl');
	labelTitulo = document.createElement('div');
	labelTitulo.setAttribute('class', 'labelTitulo');
	labelTitulo.innerHTML = 'Proyecto';
	$(boxControl).append(labelTitulo);
	cboProyecto = document.createElement('div');
	cboProyecto.setAttribute('id','divCboProy');
	cboProyecto.setAttribute('class','boxCombo');
	$(boxControl).append(cboProyecto);
	$(boxControles).append(boxControl);

    //Filtro
	boxControl = document.createElement('div');
	boxControl.setAttribute('class', 'boxControl');
	labelTitulo = document.createElement('div');
	labelTitulo.setAttribute('class', 'labelTitulo');
	labelTitulo.innerHTML = 'Criterio de selección';
	$(boxControl).append(labelTitulo);
	cboFiltros = document.createElement('select');
	cboFiltros.setAttribute('id', 'ddl_BuscarPor');
	cboFiltros.setAttribute('class', 'boxCombo');
	cboFiltros.setAttribute('onchange', 'cargarCamposBuscar()');
	$(boxControl).append(cboFiltros);
	$(boxControles).append(boxControl);


	//Campo busca
	 boxControl = document.createElement('div');
	 boxControl.setAttribute('class', 'boxControl');
	 labelTitulo = document.createElement('div');
	 labelTitulo.setAttribute('id', 'campoTit1');
	 labelTitulo.setAttribute('class', 'labelTitulo');
	 $(boxControl).append(labelTitulo);
	 inputValor =  document.createElement('input');
	 inputValor.setAttribute('id', 'txtCampo1');
	 $(boxControl).append(inputValor);
	 $(boxControles).append(boxControl);

	//Boton buscar
	boxControl = document.createElement('div');
	boxControl.setAttribute('class', 'boxControlBuscar');
	btnBuscarExp = document.createElement('div');
	btnBuscarExp.setAttribute('id', 'btnBuscarExp');
	btnBuscarExp.setAttribute('class', 'btnBuscarExp');
	$(boxControl).append(btnBuscarExp);
	$(boxControles).append(boxControl);

	var boxDonutsProyectos = document.createElement('div');
	boxDonutsProyectos.setAttribute('class', 'boxDonutsProyectos');
	boxDonutsProyectos.setAttribute('id', 'boxDonutsProyectos');
	$(this.mainFrame).append(boxDonutsProyectos);


	btnBuscarExp.onclick = function() {
		buscarExpediente();
	}

	inputValor.onkeypress = function() {
		if (event.which == 13) {
			event.preventDefault();
			buscarExpediente();
		}
	}

	buscarProyectos();
}

function buscarProyectos() {
	$.ajax({
       url: dominio_PRODUCTO + 'Global/combos.aspx',
       cache: false,
       type: 'POST',
       dataType: 'text',
       data: {
           combo: 4
       },
       xhrFields: {
           withCredentials: true
       }
   }).done(function (resultado) {
       $("#divCboProy").html(resultado);
	   cargarTipoFiltros();
   }).fail(function (jqXHR, textStatus) {
       alertLM.show(document.body, "Se han presentado inconvenientes en la aplicación. <br>El error ha sido reportado a la central.");
   });
}


function buscarMunicipios(_hostdiv) {
	var codi_dep = $("#ddl_Departamento").val();
	$.ajax({
       url: dominio_PRODUCTO + 'Global/combosSeguros.aspx',
       cache: false,
       type: 'POST',
       dataType: 'json',
       data: {
           combo: 2,
		   codi_dep: codi_dep,
		   hostdiv: _hostdiv
       },
       xhrFields: {
           withCredentials: true
       }
   }).done(function (result) {
       //$("#divCboMuni").html(resultado);
	   if (result.codigoError === "0") {
		    $('#'+_hostdiv).empty();
	   		crearComboECO(result.data[0]);
	   } else {
	   		alertLM.show(document.body, result.mensajeError);
	   }
   }).fail(function (jqXHR, textStatus) {
       alertLM.show(document.body, "Se han presentado inconvenientes en la aplicación. <br>Reportar error a la central.");
   });
}


function verHistoriaExpediente(_idCliente, _idExpediente, _textoGuia) {
	crearCargando();
	$.ajax({
       url: dominio_PRODUCTO + 'Paginas/Expediente/historiaExpediente.aspx',
       cache: false,
       type: 'POST',
       dataType: 'text',
       data: {
           idCliente: _idCliente,
		   idExpediente: _idExpediente
       },
       xhrFields: {
           withCredentials: true
       }
   }).done(function (result) {
	   eliminarCargando();
	   //<JBorrero 20-02-2019> Controlar la salida cuando se borra la session
	   if(result.indexOf("window.top.loca") >= 0) {
		   $('body').append(result);
		   return;
	   }
	   result = JSON.parse(result);
	   if (result.codigoError === "0") {
		   	ejecutarRespuestaAJAX(result);
			$('#zonaboxTituloWindow').append('<b>HISTORIA</b>  - ' + _textoGuia);
	   } else {
	   		alertLM.show(document.body, result.mensajeError);
	   }
   }).fail(function (jqXHR, textStatus) {
	   eliminarCargando();
       alertLM.show(document.body, "Se han presentado inconvenientes en la aplicación. <br>El error ha sido reportado a la central.");
   });
}

function verDocumentosExpediente(_idCliente, _idExpediente, _textoGuia) {
	crearCargando();
	$.ajax({
       url: dominio_PRODUCTO + 'Paginas/Expediente/documentosExpediente.aspx',
       cache: false,
       type: 'POST',
       dataType: 'json',
       data: {
           idCliente: _idCliente,
		   idExpediente: _idExpediente
       },
       xhrFields: {
           withCredentials: true
       }
   }).done(function (result) {
	   eliminarCargando();
	   if (result.codigoError === "0") {
	   	 ejecutarRespuestaAJAX(result);
	   	 $('#zonaboxTituloWindow').append('<b>DOCUMENTOS</b>  - ' + _textoGuia);
		 //$('#deta_imagenes_1').trigger('click');
		 //$('.boxItemAcordeon:nth-child(2) .boxTitleHeader').trigger('click')
		 $('#zonaboxAcordeonDocs .boxItemAcordeon:nth-child(2) .boxTitleHeader').trigger('click');
	   } else {
	   	 alertLM.show(document.body, result.mensajeError);
	   }
   }).fail(function (jqXHR, textStatus) {
	   eliminarCargando();
       alertLM.show(document.body, "Se han presentado inconvenientes en la aplicación. <br>El error ha sido reportado a la central.");
   });
}

function mostrarImagenExpediente(_urlCdp, _idExpedienteNE, _idCliente, _idExpediente, _idDocumentoNE, _idGestorDocNE, _idProceso, _idDocumento, _idGestorDoc) {
	crearCargando();
	if(_idGestorDocNE == 0) {
		$('#zonaboxVisor').empty();
		$('#zonaboxVisor').html("<iframe id='frm_cdpplantilla' name='frm_cdpplantilla' style='width:100%; height:100%; border:solid 1px rgba(0,0,0,0.1); margin-bottom:0px'></iframe>");
		$("#frm_cdpplantilla").attr("src", _urlCdp +"vis_doc_ima2.jsp?id_controlpro="+_idExpedienteNE+"&id_docs_caso="+_idDocumentoNE+"&proceso="+_idProceso);
		eliminarCargando();
	}
	else {
		$.ajax({
			url: dominio_PRODUCTO + 'Paginas/Expediente/visorDocumentosExp.aspx',
			cache: false,
			type: 'POST',
			dataType: 'json',
			data: {
				idCliente: _idCliente,
				idExpediente: _idExpediente,
				idDocumento: _idDocumento,
				idGestorDoc: _idGestorDoc,
				idProceso: _idProceso
			},
			xhrFields: {
				withCredentials: true
			}
		}).done(function (result) {
			eliminarCargando();
			if (result.codigoError === "0") {
				$('#zonaboxVisor').empty();
				ejecutarRespuestaAJAX(result);
				//crearVisorECO(result.data[0]);
			} else {
				alertLM.show(document.body, result.mensajeError);
			}
		}).fail(function (jqXHR, textStatus) {
			eliminarCargando();
			alertLM.show(document.body, "Se han presentado inconvenientes en la aplicación. <br>Reporte error a la central.");
		});
	}
}



function visorImagenes(){
	//Agregar en linea 52 de ECO-visorIMG
	//contain = $(_json.hostDiv);
	//contain.append(boxImgViewer);

	//Json del acordeon ECO.
	var jsonAcordeonECO = {
	    "id": "conf_68833",
	    "hostDiv": "containVIEWER1",
	    "titleAcordeon": "Edificios",
	    "classAcordeon": "clsEdificio",
	    "fnCallback": "",
	    "search": "true",
	    "pages": {
	        "npages": "10",
	        "pageActual": "4",
	        "next": "",
	        "back": "",
	        "goto": ""
	    },
	    "items": [
	        {
	            "idItem": "item1",
	            "titleItem": "Configuración Producto",
	            "expanded": "true",
	            "type": "separador",
	            "notify": "texto de la observación de la notificación 1.#OK#ayer",
	            "HTMLLabel": "<div>",
	            "HTMLResult": "",
	            "HTMLData": [
	                {
	                    "id": "d1",
	                    "campo": "RAZON SOCIAL",
	                    "valor": "JOSE GABRIEL",
	                    "data": "",
	                    "columns": "2",
	                    "type": "multilinea",
	                    "btns": [
	                        {
	                            "text": "remove#eliminar",
	                            "id": "btn2",
	                            "state": "hover",
	                            "type": "",
	                            "action": "",
	                            "data": ""
	                        }
	                    ]
	                },
	                {
	                    "id": "d1",
	                    "campo": "RAZON SOCIAL",
	                    "valor": "JOSE GABRIEL",
	                    "data": "",
	                    "columns": "2",
	                    "type": "multilinea",
	                    "btns": [
	                        {
	                            "text": "remove#eliminar",
	                            "id": "btn2",
	                            "state": "hover",
	                            "type": "",
	                            "action": "",
	                            "data": ""
	                        }
	                    ]
	                }
	            ],
	            "HTMLGrid": [
	                {
	                    "id": "fila1",
	                    "data": {},
	                    "type": "header",
	                    "columns": [
	                        {
	                            "id": "valA",
	                            "valor": "prueba 1"
	                        },
	                        {
	                            "id": "valB",
	                            "valor": "prueba 2"
	                        },
	                        {
	                            "id": "valC",
	                            "valor": "prueba 3"
	                        },
	                        {
	                            "id": "valD",
	                            "valor": "prueba 4"
	                        }
	                        ,
	                        {
	                            "id": "valD",
	                            "valor": "prueba 5"
	                        }
	                    ]
	                },
	                {
	                    "id": "fila2",
	                    "data": {},
	                    "columns": [
	                        {
	                            "id": "valA1",
	                            "valor": "X1"
	                        },
	                        {
	                            "id": "valB2",
	                            "valor": "X2"
	                        },
	                        {
	                            "id": "valC3",
	                            "valor": "X3"
	                        },
	                        {
	                            "id": "valD4",
	                            "valor": "X4"
	                        }
	                        ,
	                        {
	                            "id": "valD",
	                            "valor": "X5"
	                        }
	                    ]
	                },
	                {
	                    "id": "fila3",
	                    "data": {},
	                    "columns": [
	                        {
	                            "id": "valA12",
	                            "valor": "Y1"
	                        },
	                        {
	                            "id": "valB22",
	                            "valor": "Y2"
	                        },
	                        {
	                            "id": "valC32",
	                            "valor": "Y3"
	                        },
	                        {
	                            "id": "valD42",
	                            "valor": "Y4"
	                        }
	                        ,
	                        {
	                            "id": "valD",
	                            "valor": "Y5"
	                        }
	                    ]
	                }
	            ],
	            "HTMLMosaic": [
	                {
	                    "id": "d1",
	                    "campo": "RAZON SOCIAL",
	                    "label": "10 de Febrero 2018 / 05:30 pm",
	                    "valor": "JOSE GABRIEL",
	                    "data": "",
	                    "columns": "2",
	                    "size": "big",
	                    "state": "",
	                    "bgimage": "./img/userLog.svg",
	                    "action": "",
	                    "type": "multilinea",
	                    "btns": [
	                        {
	                            "text": "remove#eliminar",
	                            "id": "btn2",
	                            "state": "hover",
	                            "type": "",
	                            "action": "",
	                            "data": ""
	                        }
	                    ]
	                }
	            ],
	            "data": { "idTram": "lsdkfjlskdf", "count": "40" },
	            "btns": [
	                {
	                    "text": "conf#Configuración del modulo",
	                    "id": "btn1",
	                    "state": "active",
	                    "type": "notify",
	                    "action": "",
	                    "data": {
	                        "idLoad": "load_1234",
	                        "fileType": ["pdf","jpg","png"],
	                        "maxSize": "1000",
	                        "minCantArch": "1",
	                        "maxCantArch": "5",
	                        "ambiente": "pruebas",
	                        "urlSan": "",
	                        "onComplete": "alert('termino')",
	                        "error": "alert('error al cargar')"
	                    }
	                },
	                {
	                    "text": "remove#eliminar",
	                    "id": "btn2",
	                    "state": "hover",
	                    "type": "",
	                    "action": "",
	                    "data": ""
	                },
	                {
	                    "text": "Configurar",
	                    "id": "btn3",
	                    "state": "active",
	                    "type": "",
	                    "action": "",
	                    "data": ""
	                }
	            ],
	            "action": ""
	        }
	    ]
	};

	crearAcordeonECO(jsonAcordeonECO);
}


function mostrarDetalleActosNotificar(_idCliente, _cc_per, _item, _continua) {
	if (_continua == 'NO') {
		alertLM.show(document.body, "Para ver notificaciones debe realizar consultas por placa");
		return;
	}

    crearCargando();
	$.ajax({
       url: dominio_PRODUCTO + 'Paginas/Expediente/detalleActosNotificar.aspx',
       cache: false,
       type: 'POST',
       dataType: 'json',
       data: {
           id_cliente: _idCliente,
     	   cc_per: _cc_per,
           placa: _item
       },
       xhrFields: {
           withCredentials: true
       }
	}).done(function (result) {
	   	eliminarCargando();
		if (result.codigoError === "0") {
			ejecutarRespuestaAJAX(result);
	      	$('#zonaboxTituloWindow').append('<b>NOTIFICAR</b>  - ' + _item);
	    } else {
	      alertLM.show(document.body, result.mensajeError);
	    }
    }).fail(function (jqXHR, textStatus) {
       eliminarCargando();
       alertLM.show(document.body, "Se han presentado inconvenientes en la aplicación. <br>El error ha sido reportado a la central.");
	});
}

function imprimirNotificacion() {
	//Imprimimos notificacion si no hay no esta activo el tema de
	//Firma con tableta-Si imprime y se adjuntaria
	//generarImpesionNotificacion();
	mostrarControlesEvidenciaNotif();
}

function mostrarControlesEvidenciaNotif() {
	crearCargando();
	$.ajax({
		url: dominio_PRODUCTO + 'Paginas/Expediente/controlesNotificacion.aspx',
		cache: false,
		type: 'POST',
		dataType: 'json',
		data: {
			idCliente: 1,
			idExpediente: 100
		},
		xhrFields: {
			withCredentials: true
		}
	}).done(function (result) {
		eliminarCargando();
		if (result.codigoError === "0") {
			ejecutarRespuestaAJAX(result);
			$("#zonaboxPorNotificar").animate({
					 height: '70px'
				 }, 500 );
			$("#zonaboxPorNotificar").css('filter','blur(1px)');
		} else {
		   alertLM.show(document.body, result.mensajeError);
		}
	}).fail(function (jqXHR, textStatus) {
		eliminarCargando();
		alertLM.show(document.body, "Se han presentado inconvenientes en la aplicación. <br>Reporte error a la central.");
	});
}


function showListadoPorNotificar() {
	$("#zonaboxPorNotificar").animate({
			 height: '100%'
		 }, 500 );
	 $("#zonaboxPorNotificar").css('filter','none');
     $('#zonaboxEvidencia').empty();
}

function guardarRecibidoNotificacion() {
	var direccion = $('#txtDireccion').val();
	var cod_depto = $('#ddl_Departamento').val();
	var cod_muni  = $('#ddl_municipio').val();
	var telefono  = $('#txtTelefono').val();
	var correo    = $('#txtCorreo').val();

	crearCargando();
	$.ajax({
		url: dominio_PRODUCTO + 'Paginas/Expediente/guardarRecibidoNotificacion.aspx',
		cache: false,
		type: 'POST',
		dataType: 'text',
		data: {
			direccion: direccion,
			cod_depto: cod_depto,
			cod_muni: cod_muni,
			telefono: telefono,
			correo: correo
		},
		xhrFields: {
			withCredentials: true
		}
	}).done(function (resultado) {
		eliminarCargando();
		alert(resultado)
	}).fail(function (jqXHR, textStatus) {
		eliminarCargando();
		alertLM.show(document.body, "Se han presentado inconvenientes en la aplicación. <br>Reporte error a la central.");
	});
}

function mostrarFiltroExpedientePopOver() {
	//$('[data-toggle="popover"]').popover();
	$('#btnPopOverFiltro').attr('data-content', '<li><input type="checkbox" id="chkFilAbiertos" name="chkFilAbiertos" value="abiertos" onchange="buscarExpediente(true, this)"> Solo abiertos</li><li><input type="checkbox" id="chkFilFiscal" name="chkFilFiscal" value="fiscal" onchange="buscarExpediente(true,this)"> En fiscalización</li><li><input type="checkbox" id="chkFilCoactivo" name="chkFilCoactivo" value="coactivo" onchange="buscarExpediente(true,this)"> En coactivo</li><li class="divider"></li>');
	// eliminarCargando();
	$('[data-toggle="popover"]').popover();
	//<div class="btn-group"><button type="button" class="boxCheckAbierto glyphicon glyphicon-filter" data-toggle="dropdown"><span class="caret"></span></button><ul class="dropdown-menu" role="menu"><li><input type="checkbox" id="chkFilAbiertos" name="chkFilAbiertos" value="abiertos" onchange="buscarExpediente(true, this)"> Solo abiertos</li><li><input type="checkbox" id="chkFilFiscal" name="chkFilFiscal" value="fiscal" onchange="buscarExpediente(true,this)"> En fiscalización</li><li><input type="checkbox" id="chkFilCoactivo" name="chkFilCoactivo" value="coactivo" onchange="buscarExpediente(true,this)"> En coactivo</li><li class="divider"></li></ul></div>
}


function verEmbargosExpediente(_idCliente, _idExpediente, _textoGuia) {
	crearCargando();
	$.ajax({
	   url: dominio_PRODUCTO + 'Paginas/Expediente/datosConsultaEmbargos.aspx',
	   cache: false,
	   type: 'POST',
	   dataType: 'json',
	   data: {
		   idCliente: _idCliente,
		   idExpediente: _idExpediente,
		   textoguia: _textoGuia
	   },
	   xhrFields: {
		   withCredentials: true
	   }
	}).done(function (result) {
	   eliminarCargando();
	   if (result.codigoError === "0") {
		 ejecutarRespuestaAJAX(result);
		 $('#zonaboxTituloWindowEmb').append('<b>EMBARGOS</b>  - ' + _textoGuia);
	   } else {
		 alertLM.show(document.body, result.mensajeError);
	   }
	}).fail(function (jqXHR, textStatus) {
	   eliminarCargando();
	   alertLM.show(document.body, "Se han presentado inconvenientes en la aplicación. <br>Intente nuevamente y reporte el error a la central.");
	});
}

function verDetalleEmbargo(_idCliente, _idExpedienteEmb, _textoGuia) {
	crearCargando();
	$.ajax({
		url: dominio_PRODUCTO + 'Paginas/Expediente/detalleEmbargo.aspx',
		cache: false,
		type: 'POST',
		dataType: 'json',
		data: {
			idCliente: _idCliente,
			idExpedienteEmb : _idExpedienteEmb
		},
		xhrFields: {
			withCredentials: true
		}
	}).done(function (result) {
		eliminarCargando();
		$('#zonaBoxDerAcordeonEmb').empty();
		if (result.codigoError === "0") {
			ejecutarRespuestaAJAX(result);
			$('#zonaboxTituloDetalleEmb').text(_textoGuia);
		} else {
			alertLM.show(document.body, result.mensajeError);
		}
	}).fail(function (jqXHR, textStatus) {
		eliminarCargando();
		alertLM.show(document.body, "Se han presentado inconvenientes en la aplicación. <br>Reporte error a la central.");
	});
}

function verHistoriaEmbargo(_idCliente, _idExpedienteEmb, _textoGuia) {
	crearCargando();
	$.ajax({
	   url: dominio_PRODUCTO + 'Paginas/Expediente/historiaEmbargo.aspx',
	   cache: false,
	   type: 'POST',
	   dataType: 'json',
	   data: {
		   idCliente: _idCliente,
		   idExpedienteEmb: _idExpedienteEmb,
		   textoguia: _textoGuia
	   },
	   xhrFields: {
		   withCredentials: true
	   }
	}).done(function (result) {
	   eliminarCargando();
	   if (result.codigoError === "0") {
			ejecutarRespuestaAJAX(result);
			//$('#zonaboxTituloWindowEmbHis').append('<b>HISTORIA</b>  - ' + _textoGuia);
	   } else {
			alertLM.show(document.body, result.mensajeError);
	   }
	}).fail(function (jqXHR, textStatus) {
	   eliminarCargando();
	   alertLM.show(document.body, "Se han presentado inconvenientes en la aplicación. <br>Reporte error a la central.");
	});
}

function cerrarAcodeones(that, _boxName) {
	$('[id*='+_boxName+']').css('display','none');
}
