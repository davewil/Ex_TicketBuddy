export function waitUntil(condition_function: () => boolean) {
    const poll = (resolve: any) => {
        if (condition_function()) resolve()
        else setTimeout(() => poll(resolve), 400);
    }

    return new Promise(poll);
}

export const delay = (millis: number) => new Promise((resolve) => {
    // @ts-ignore
    setTimeout(_ => resolve(_), millis)
});