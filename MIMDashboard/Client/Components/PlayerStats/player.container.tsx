import {connect} from "react-redux";
import Dashboard from "../Dashboard";
import { GET_PLAYERS  }from "./player.action";

const mapStateToProps = (state: any) => {
    return state.stats
};

const mapDispatchToProps = (dispatch: any, ownProps: any) => ({
    getPlayer: () => {
        dispatch({type:'GET_PLAYERS', data: {ownProps, loading:true}});
    } 
});

export default connect(mapStateToProps, mapDispatchToProps)(Dashboard);
