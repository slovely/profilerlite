import * as Classes from "./classes";
import * as Enums from "./enums";
import ProfilerLite = Classes.ProfilerLite;

import {autoinject} from "aurelia-dependency-injection";
import {HttpClient, json, RequestInit} from "aurelia-fetch-client";


  export interface IDictionary<T> {
     [key: string]: T;
  }

  @autoinject
  export class Data {
    constructor(private http: HttpClient) {
    }
    public sessionDetail(id: number, ajaxOptions: RequestInit|undefined = undefined): PromiseLike<ProfilerLite.Core.Models.DatabaseSessionDetail> {
      const options: RequestInit = { 
        method: "get", 
        body: null ? json(null) : undefined
      };
      if (ajaxOptions) Object.assign(options, ajaxOptions);
      return this.http.fetch("api/Data/sessionDetail/" + id, options)
        .then((response: Response) => (response && response.status!==204) ? response.json() : null);
    }

    public sessions(urlFilter: string, ajaxOptions: RequestInit|undefined = undefined): PromiseLike<Array<ProfilerLite.Core.Models.DatabaseSessionSummary>> {
      const options: RequestInit = { 
        method: "get", 
        body: null ? json(null) : undefined
      };
      if (ajaxOptions) Object.assign(options, ajaxOptions);
      return this.http.fetch("api/Data/sessions" + "?urlFilter=" + urlFilter + "", options)
        .then((response: Response) => (response && response.status!==204) ? response.json() : null);
    }

  }
  @autoinject
  export class WeatherForecast {
    constructor(private http: HttpClient) {
    }
    public get(a: Enums.ProfilerLite.Core.Models.DummyEnumToFixTypeScriptGenerationBug, ajaxOptions: RequestInit|undefined = undefined): PromiseLike<Array<ProfilerLite.Core.Models.WeatherForecast>> {
      const options: RequestInit = { 
        method: "get", 
        body: null ? json(null) : undefined
      };
      if (ajaxOptions) Object.assign(options, ajaxOptions);
      return this.http.fetch("api/WeatherForecast/get" + "?a=" + a + "", options)
        .then((response: Response) => (response && response.status!==204) ? response.json() : null);
    }

  }
