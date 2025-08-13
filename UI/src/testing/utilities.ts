export function waitUntil(condition_function: () => boolean) {
    const poll = (resolve: (value: unknown) => void) => {
        if (condition_function()) resolve({})
        else setTimeout(() => poll(resolve), 400);
    }

    return new Promise(poll);
}

export const delay = (millis: number) => new Promise((resolve) => {
    setTimeout(_ => resolve(_), millis)
});