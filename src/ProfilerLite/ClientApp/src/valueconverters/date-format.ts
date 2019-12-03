import * as moment from "moment";

export class DateFormatValueConverter {
    public toView(value: string, format: string = "DD/MM/YYYY HH:mm:ss"): string {
        if (!value) return "";
        return moment(new Date(value)).format(format);
    }
}
