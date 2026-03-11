import autoAnimate from '@formkit/auto-animate'

document.addEventListener('DOMContentLoaded', () => {
    const list = document.querySelector('[data-auto-animate]')
    if (list) {
        autoAnimate(list)
    }
})

document.addEventListener("DOMContentLoaded", () => {
    const table = document.getElementById("table-body")
    if (table) {
        autoAnimate(table)
    }
})