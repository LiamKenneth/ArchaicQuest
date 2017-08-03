import { combineReducers } from 'redux';
import { fetchSuccess, fetchErrored, fetchLoading } from '../actions/player.action';

export default combineReducers({
 fetchSuccess, 
 fetchErrored,
  fetchLoading
});
