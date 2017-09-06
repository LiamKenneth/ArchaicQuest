export function fetchErrored(bool: boolean) {
    return {
        type: 'FETCH_ERRORED',
        hasErrored: bool
    };
}

export function fetchLoading(bool: boolean) {
    return {
        type: 'FETCH_LOADING',
        isLoading: bool
    };
}

export function fetchSuccess(items: any) {
    return {
        type: 'FETCH_SUCCESS',
        items
    };
}

export function FetchData(url: string) {
    return (dispatch: any) => {
        dispatch(fetchLoading(true));
 
         fetch(url)
            .then(response =>  response.json(),
              error => console.log('An error occured.', error)
        
            )
            .then(json => {
      
              dispatch(fetchLoading(false));
            dispatch(fetchSuccess(json));
                console.log(json)

            })
            .catch(() => dispatch(fetchErrored(true)));
            
    };
}
