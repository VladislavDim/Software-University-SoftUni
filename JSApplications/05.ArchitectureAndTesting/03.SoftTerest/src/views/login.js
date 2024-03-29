import { login } from "../api/users.js";

const section = document.querySelector('#loginPage');
const form = section.querySelector('form');
form.addEventListener('submit', onSubmit);

let ctx;

export function showLogin(context) {
    ctx = context;
    context.showSection(section);
}

async function onSubmit(e) {
    e.preventDefault();

    const formData = new FormData(form);

    const email = formData.get('email');
    const password = formData.get('password');

    await login(email, password);
    ctx.goto('/');
} 