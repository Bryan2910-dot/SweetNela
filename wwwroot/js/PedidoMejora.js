function seleccionarSabor(sabor) {
  document.getElementById("sabor").value = sabor;

  // Remover selección previa
  let botones = document.querySelectorAll(".sabor-opcion");
  botones.forEach((btn) => btn.classList.remove("seleccionado"));

  // Resaltar la opción seleccionada
  let botonSeleccionado = [...botones].find(
    (btn) => btn.innerText.trim() === sabor
  );
  if (botonSeleccionado) {
    botonSeleccionado.classList.add("seleccionado");
  }
}
// --- para calcular ---
document.addEventListener("DOMContentLoaded", function () {
  const btnCalcular = document.getElementById("btnCalcular");
  if (btnCalcular) {
    btnCalcular.addEventListener("click", function () {
      const form = document.querySelector("form");
      const formData = new FormData(form);
      const data = {};
      formData.forEach((value, key) => {
        data[key] = value;
      });

      fetch("/PedidoMejora/CalcularPrecio", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
      })
        .then((response) => response.json())
        .then((result) => {
          document.getElementById(
            "resultadoPrecio"
          ).innerText = `El precio total es: ${result.precio}`;
        })
        .catch(() => {
          document.getElementById("resultadoPrecio").innerText =
            "Error al calcular el precio.";
        });
    });
  }
});
