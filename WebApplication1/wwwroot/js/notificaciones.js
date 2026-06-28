(function () {
    var notifBadge = document.getElementById("notifBadge");
    var notifList = document.getElementById("notifList");
    var sinNotif = document.getElementById("sinNotif");
    var notifDropdown = document.getElementById("notifDropdown");

    var conexion = new signalR.HubConnectionBuilder()
        .withUrl("/hubs/chat")
        .withAutomaticReconnect()
        .build();

    function ocultarSiCero(valor) {
        if (!notifBadge) return;
        notifBadge.textContent = valor;
        notifBadge.style.display = valor > 0 ? "inline" : "none";
    }

    function agregarNotificacion(data) {
        if (sinNotif) sinNotif.remove();
        ocultarSiCero((parseInt(notifBadge.textContent) || 0) + 1);

        var a = document.createElement("a");
        a.href = data.url || "#";
        a.className = "dropdown-item border-bottom py-2";
        a.innerHTML = '<div class="d-flex align-items-center">'
            + '<div class="flex-shrink-0 me-2">'
            + '<i class="bi ' + (data.tipo === "oferta" ? "bi-hand-index" : "bi-chat-dots") + ' fs-5 text-secondary"></i>'
            + "</div>"
            + '<div class="flex-grow-1 text-truncate">'
            + "<small>" + (data.mensaje || "Nueva notificación") + "</small>"
            + "</div>"
            + "</div>";
        notifList.insertBefore(a, notifList.firstChild);
    }

    conexion.on("NuevaNotificacion", function (data) {
        agregarNotificacion(data);
    });

    conexion.start().catch(function (err) {
        console.error("Error notif SignalR:", err.toString());
    });

    if (notifDropdown) {
        notifDropdown.addEventListener("show.bs.dropdown", function () {
            ocultarSiCero(0);
        });
    }
})();
