(function () {
    var contenedor = document.getElementById("chatMessages");
    if (!contenedor) return;

    var conversacionId = parseInt(contenedor.getAttribute("data-conversacion-id"));
    var usuarioLogueado = parseInt(contenedor.getAttribute("data-usuario-logado"));
    var coloresInit = contenedor.getAttribute("data-colores");

    var paleta = ["#6f42c1", "#fd7e14", "#20c997", "#dc3545", "#0dcaf0", "#198754", "#d63384", "#0d6efd", "#6610f2", "#e83e8c"];
    var coloresConocidos = {};
    var colorIdx = 0;

    if (coloresInit) {
        try {
            coloresConocidos = JSON.parse(coloresInit);
            colorIdx = Object.keys(coloresConocidos).length;
        } catch (e) {}
    }

    function colorParaUsuario(usuarioId) {
        var key = String(usuarioId);
        if (!coloresConocidos[key]) {
            coloresConocidos[key] = paleta[colorIdx % paleta.length];
            colorIdx++;
        }
        return coloresConocidos[key];
    }

    function escapeHtml(texto) {
        var d = document.createElement("div");
        d.appendChild(document.createTextNode(texto));
        return d.innerHTML;
    }

    var conexion = new signalR.HubConnectionBuilder()
        .withUrl("/hubs/chat")
        .withAutomaticReconnect()
        .build();

    conexion.on("RecibirMensaje", function (msg) {
        var div = document.createElement("div");
        var esPropio = msg.usuarioId === usuarioLogueado;

        div.className = (esPropio ? "d-flex justify-content-end" : "d-flex justify-content-start") + " mb-3 mensaje-item";
        div.setAttribute("data-usuario", msg.usuarioId);

        var fecha = new Date(msg.fechaEnvio);
        var fechaStr = fecha.toLocaleDateString("es-AR") + " " + fecha.toLocaleTimeString("es-AR", { hour: "2-digit", minute: "2-digit" });

        if (esPropio) {
            div.innerHTML = '<div class="bg-primary text-white rounded p-3">'
                + "<strong>Vos</strong><br />"
                + escapeHtml(msg.texto) + "<br />"
                + "<small>" + fechaStr + "</small></div>";
        } else {
            var color = colorParaUsuario(msg.usuarioId);
            div.innerHTML = '<div class="rounded p-3 text-white" style="background-color:' + color + ';">'
                + "<strong>" + escapeHtml(msg.usuarioNombre) + "</strong><br />"
                + escapeHtml(msg.texto) + "<br />"
                + '<small class="text-white-50">' + fechaStr + "</small></div>";
        }

        contenedor.appendChild(div);
        contenedor.scrollTop = contenedor.scrollHeight;
    });

    conexion.start().then(function () {
        return conexion.invoke("JoinConversacion", conversacionId);
    }).catch(function (err) {
        console.error("Error chat SignalR:", err.toString());
    });

    var input = document.getElementById("chatInput");
    var btn = document.getElementById("btnEnviarChat");

    function enviar() {
        var texto = input.value.trim();
        if (texto === "") return;

        btn.disabled = true;
        conexion.invoke("EnviarMensaje", conversacionId, texto)
            .then(function () {
                input.value = "";
                input.focus();
            })
            .catch(function (err) {
                console.error("Error al enviar:", err.toString());
            })
            .finally(function () {
                btn.disabled = false;
            });
    }

    if (btn) {
        btn.addEventListener("click", enviar);
    }

    if (input) {
        input.addEventListener("keydown", function (e) {
            if (e.key === "Enter") {
                e.preventDefault();
                enviar();
            }
        });
    }
})();
