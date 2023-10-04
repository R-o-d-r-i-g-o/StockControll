function onlyNumbers(value) {
    return value.replace(/[^\d]/g, '');
}

function RemoveSpecialCharacters(input) {
    var cleanedValue = onlyNumbers(input.value);
    input.value = cleanedValue;
}
