function CarregaIdChamado(idChamado, deStatus, paraStatus) {
    //window.alert(idChamado + " " + deStatus + " " + paraStatus);
    document.getElementById("deStatus").setAttribute('value', deStatus);
    document.getElementById("paraStatus").setAttribute('value', paraStatus);
    document.getElementById("idChamadoHist").setAttribute('value', idChamado);
    document.getElementById("filtroempresa1").setAttribute('value', document.getElementById("empresa").value);
    document.getElementById("filtrotipochamado1").setAttribute('value', document.getElementById("tipochamado").value);
    document.getElementById("filtroatividadechamado1").setAttribute('value', document.getElementById("atividadechamado").value);
    document.getElementById("filtroidchamado1").setAttribute('value', document.getElementById("idchamado").value);
    document.getElementById("filtroStatusPesq1").setAttribute('value', document.getElementById("StatusPesq").value);
    document.getElementById("idChamadoAdiar").setAttribute('value', idChamado);

};
function CarregaFiltros() {
    document.getElementById("filtroempresa").setAttribute('value', document.getElementById("empresa").value);
    document.getElementById("filtrotipochamado").setAttribute('value', document.getElementById("tipochamado").value);
    document.getElementById("filtroatividadechamado").setAttribute('value', document.getElementById("atividadechamado").value);
    document.getElementById("filtroidchamado").setAttribute('value', document.getElementById("idchamado").value);
    document.getElementById("filtroStatusPesq").setAttribute('value', document.getElementById("StatusPesq").value);

};

