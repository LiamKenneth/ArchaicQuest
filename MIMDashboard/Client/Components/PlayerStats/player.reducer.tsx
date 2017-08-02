import { GET_PLAYERS } from 'player.action';

const initialState = {
  players: Array
}

function DashboardAction(state : any = initialState, action : any) {

    switch (action.type) {
        case GET_PLAYERS:
        return Object.assign({}, state)
        default:
        return state
    }
 
}