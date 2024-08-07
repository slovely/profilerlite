import {autoinject} from "aurelia-dependency-injection";
import {HttpClient} from "aurelia-fetch-client";
import * as Actions from './server/actions';
import {ProfilerLite} from "./server/classes";
import * as moment from "moment";

@autoinject
export class App {
    public sessions: Array<ProfilerLite.Core.Models.DatabaseSessionSummary> = [];
    public selectedSession: ProfilerLite.Core.Models.DatabaseSessionDetail = null;
    public selectedQuery: ProfilerLite.Core.Models.DatabaseQuery = null;
    public urlFilter: string = "";

    constructor(private fetchClient: HttpClient, private dataCtrl: Actions.Data){
    }
    
    public async activate() {
        await this.loadSessions();
    }

    private async loadSessions() {
        const sessions2 = await this.dataCtrl.sessions(this.urlFilter);
        console.log(sessions2.length);
        this.sessions = sessions2;
    }

    public async selectSession(sessionId: number){
        this.selectedSession = await this.dataCtrl.sessionDetail(sessionId);
    }

    public async selectQuery(query: ProfilerLite.Core.Models.DatabaseQuery) {
        this.selectedQuery = query;
    }
    
    public async clearDatabase() {
        this.dataCtrl.clearDatabase();
    }
    
    public async downloadAll() {
        //this.dataCtrl.downloadAllQueries(this.selectedSession.id);
        window.location.href = "/api/data/downloadallqueries/" + this.selectedSession.id;
    }
}
