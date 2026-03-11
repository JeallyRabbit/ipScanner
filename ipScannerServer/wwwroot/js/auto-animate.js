import autoAnimate from 'https://cdn.jsdelivr.net/npm/@formkit/auto-animate/+esm';

export function enableAutoAnimate(selector) {
    const el = document.querySelector(selector);
    if (el) {
        autoAnimate(el);
    }
}

document.addEventListener("DOMContentLoaded", () => {
    const table = document.getElementById("table-body")
    if (table) {
        autoAnimate(table)
    }
})