export function itemsHasErrored(state = false, action: any) {
    switch (action.type) {
        case 'FETCH_ERRORED':
            return action.hasErrored;

        default:
            return state;
    }
}

export function itemsIsLoading(state = false, action: any) {
    switch (action.type) {
        case 'FETCH_LOADING':
            return action.isLoading;

        default:
            return state;
    }
}

export function items(state = {}, action: any) {
    switch (action.type) {
        case 'FETCH_SUCCESS':
            return action.items;

        default:
            return state;
    }
}
