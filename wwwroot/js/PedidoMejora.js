function seleccionarSabor(sabor) {
    document.getElementById("sabor").value = sabor;

    // Remover selección previa
    let botones = document.querySelectorAll(".sabor-opcion");
    botones.forEach(btn => btn.classList.remove("seleccionado"));

    // Resaltar la opción seleccionada
    let botonSeleccionado = [...botones].find(btn => btn.innerText.trim() === sabor);
    if (botonSeleccionado) {
        botonSeleccionado.classList.add("seleccionado");
    }
}