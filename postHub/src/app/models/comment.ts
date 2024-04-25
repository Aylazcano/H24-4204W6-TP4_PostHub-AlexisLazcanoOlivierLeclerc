export class Comment{
    constructor(
        public id : number,
        public text : string,
        public date : Date,
        public username : string | null,
        public upvotes : number,
        public downvotes : number,
        public upvoted : boolean,
        public downvoted : boolean,
        public subCommentTotal : number,
        public pictureIds : number[] | null,
        public subComments : Comment[] | null
    ){}
}