window.setCookie = function (name, value, expires) {
    document.cookie = `${name}=${value};expires=${expires};path=/`;
};

window.getCookie = function (name) {
    const cookieValue = document.cookie
        .split('; ')
        .find(row => row.startsWith(`${name}=`))
        ?.split('=')[1];

    return cookieValue;
};

window.deleteCookie = function (name) {
    document.cookie = `${name}=;expires=Thu, 01 Jan 1970 00:00:00 UTC;path=/;`;
};
