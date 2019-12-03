import * as __Enums from "./enums";


export module ProfilerLite.Core.Models {
	export interface DatabaseQuery {
		commandText: string;
		commandTextParameterized: string;
		id: number;
		parameters: string;
		parametersDeserialized: ProfilerLite.Core.Models.QueryParameter[];
		rows: number;
		time: number;
		timeFormatted: string;
	}
	export interface DatabaseSessionDetail extends ProfilerLite.Core.Models.DatabaseSessionSummary {
		databaseQueries: ProfilerLite.Core.Models.DatabaseQuery[];
	}
	export interface DatabaseSessionSummary {
		createdDate: Date;
		id: number;
		method: string;
		queryCount: number;
		shortenedUrl: string;
		url: string;
	}
	export interface QueryParameter {
		name: string;
		value: string;
	}
	export interface WeatherForecast {
		date: Date;
		summary: string;
		temperatureC: number;
		temperatureF: number;
	}
}
