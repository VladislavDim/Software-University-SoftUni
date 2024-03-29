class Textbox {
    constructor(selector, regex) {
        this._elements = document.querySelectorAll(selector);
        this._invalidSymbols = regex;
        [...this._elements].forEach(x => x.addEventListener('change', () => this.value = x.value));
    }

    get value() {
        return this._value;
    }
    set value(crnValue) {
        this._value = crnValue;
        Array.from(this._elements).forEach(x => x.value = crnValue);
    }

    get elements() {
        return this._elements;
    }

    isValid() {
        return !this._invalidSymbols.test(this.value);
    }
}

let textbox = new Textbox(".textbox", /[^a-zA-Z0-9]/);
let inputs = document.getElementsByClassName('textbox');

Array.from(inputs)
  .forEach(el => el.addEventListener('click', function () { console.log(el.value); }));
