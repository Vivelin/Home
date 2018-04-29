declare namespace Twitch {
    interface User {
        /** User’s broadcaster type: "partner", "affiliate", or "". */
        broadcaster_type: '' | 'partner' | 'affiliate'

        /** User’s channel description. */
        description: string

        /** User’s display name. */
        display_name: string

        /** User’s email address. Returned if the request includes the user:read:email scope. */
        email?: string

        /** User’s ID. */
        id: string

        /** User’s login name. */
        login: string

        /** URL of the user’s offline image. */
        offline_image_url: string

        /** URL of the user’s profile image. */
        profile_image_url: string

        /** User’s type: "staff", "admin", "global_mod", or "". */
        type: '' | 'staff' | 'admin' | 'global_mod'

        /** Total number of views of the user’s channel. */
        view_count: Number
    }

    interface Follow {
        /** Date and time when the `from_id` user followed the `to_id` user. */
        followed_at: string

        /** ID of the user following the `to_id` user. */
        from_id: string

        /** A cursor value, to be used in a subsequent request to specify the starting point of the next set of results. */
        pagination: string

        /** ID of the user being followed by the from_id user. */
        to_id: string

        /** 
         * Total number of items returned.
         * - If only `from_id` was in the request, this is the total number of followed users.
         * - If only `to_id` was in the request, this is the total number of followers.
         * - If both `from_id` and `to_id` were in the request, this is 1 (if the “from” user follows the “to” user) or 0. 
         */
        total: Number
    }

    interface Stream {
        community_ids: string[]
        game_id: string
        id: string
        language: string
        pagination: string
        started_at: string
        thumbnail_url: string
        title: string
        type: '' | 'live' | 'vodcast'
        user_id: string
        viewer_count: Number
    }

    interface Response<T> {
        data?: T[]
        error?: string
        status?: Number
        message?: string
    }
}