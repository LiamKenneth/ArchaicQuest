export const GET_PLAYERS = "GET_PLAYERS";

/**
 * returns all player data
 * @constructor
 * @param {any} data - player(s) data
 */
export const getPlayers = (data: any) => {
    return {
        type: GET_PLAYERS,
        data
    }
    
}