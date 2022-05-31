window.myPromiseFn = () => {
    return new Promise((resolve, reject) => {
        setTimeout(() => {
            resolve("From JS!");
        }, 1000);
    });
}

window.errorPromiseFn = () => {
    return new Promise((resolve, reject) => {
        setTimeout(() => {
            reject("Error!");

        }, 1000);
    });
}

window.complexPromiseFn = () => {
    return new Promise((resolve, reject) => {
        setTimeout(() => {
            resolve({ Name: 'Joonas' });
        }, 1000);
    });
}

window.paramPromiseFn = (data) => {
    return new Promise((resolve, reject) => {
        setTimeout(() => {
            console.dir(data);
            resolve(data.name + " processed!");
        }, 1000);
    });
};