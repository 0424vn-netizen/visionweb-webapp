
var AlertType = {
    INFORMATION: "Information",
    ACTION: "Action",
};
var AlertMode = {
    SINGLE: "single",
    GROUP: "group",
};
var ActionBarType = {
    ALERT: "Alert",
    EMAIL: "Email",
};
var AlertGroupType = {
    Category: "Category",
    Regular: "Regular",
    None: "None"
};

var Status = {
    READ: 6,
    DELETE: 7,
    VIEW: 8,
    PENDING: 5
};

var FilterStatus = {
    READ: "6",
    UNREAD: "5,8",
    All: "-1"
};

var FilterSourceApp = {
    All: "-1",
};

var SortDes = {
    ASC: "0",
    DESC: "1"
};

var AlertActionType = {
    UNREAD: "UnRead",
    TOTAL: "Total",
};

var alertComponentObj = {
    UnreadIndicator: "#btnUnread",
    TotalIndicator: "#lblTotal",
    FilterText: "#lblFilterText",
    NewFeedElm: "#btnNewFeed",
    CloseElm: "#btnCloseFeed",
    ClearElm: "#btnClearFeed",
    MarkallElm: "#btnMarkAllRead",
    ShowFilterElm: "#btnShowFilter",
    SortElm: "#btnSortAlert",
    Loading: "loading-mask-popover",
    Loading1: "loading-mask-test"
}

var alertActions = {
    MarkAllRead: "MarkAllRead",
    MarkRead: "MarkRead",
    DeleteAll: "DeleteAll",
    Delete: "Delete",
    View: "View",
    Filter: "Filter",
    Sort: "Sort",
    DismissAll: "DismissAll",
}

var subDomain = "";
var alertObj;
var ActionBarModule = function (options) {
    var self = this;
    var items = [];
    var actionBar = $.extend({
        baseHtml: "default",
        position: 'bottom-right',
        zIndex: '999',
    }, options);

    var ActionContent = {
        Delete: "Delete",
        Clear: "Clear",
        MarkRead: "Mark as Read",
        Close: "Close",
        NoItem: "No item",
        ToDay: "Today",
        Yesterday: "Yesterday",
        ThisWeek: "This Week",
        ThisMonth: "This Month",
        LastWeek: "Last Week",
        TwoWeeksAgo: "Two Weeks Ago",
        ThreeWeeksAgo: "Three Weeks Ago",
        LastMonth: "Last Month",
        Older: "Older",
        CloseText: "Close",
        ViewText: "View",
        DismissText: "Dismiss",

        CM: 'Case Management',
        ALICE: 'ALICE',
        All: 'All',
        Read: 'Read',
        UnReadText: 'Unread',

        SourceText: 'Source',
        StatusText: 'Status',
        MultipleText: 'Multiple choices',
        SingleText: 'Single choices',
    }

    var alertOption = {
        CloseText: ActionContent.CloseText,
        ViewText: ActionContent.ViewText,
        DismissText: ActionContent.DismissText,
        PendingStatus: Status.PENDING,
    };

    var sourceApps = [];

    var sourceAppsDefaultItem = {
        ID: -1,
        Name: ActionContent.All
    }


    var statuApps = [
        { ID: -1, Name: ActionContent.All },
        { ID: 6, Name: ActionContent.Read },
        { ID: 5, Name: ActionContent.UnReadText }
    ];

    self.actionContent = function (conten) {
        ActionContent = conten;
    }

    self.addItem = function (item) {
        items.push(item);
    }

    self.register = function () {
        register();
    }

    self.setSubDomain = function (value) {
        subDomain = value;
    }

    self.reLoadAlert = function () {
        reLoadAlertFeed();
        var alertModule = new AlertModule(alertOption);
        alertModule.buildAlertMsg();
    }

    self.updateFeedItem = function (alertId) {
        onUpdateFeedItemView(alertId);
    }

    var register = function () {
        alertObj = items[0];

        var urlServer = alertObj.getSourcesUri;
        var datas = {};
        var serverType = "GET";

        alertAjax(urlServer, datas, function (data) {
            if (data) {
                if (data.d.length > 1 || data.d.length == 0) {
                    sourceApps.push(sourceAppsDefaultItem);
                }

                for (var i = 0; i < data.d.length; i++) {
                    var item = {};
                    item.ID = data.d[i].SourceID;
                    item.Name = data.d[i].SourceName;
                    sourceApps.push(item);
                }

                // Init alert
                $('#div-alert').append(buildAlertButton(alertObj));
                initPopoverAlertFeed(alertObj);

                // Show alert toast
                showAlert(alertObj.getMsgUri, alertObj.updateMsgStatusUri, alertObj.alertTime, alertObj.alertTimeClose);
            }
        }, serverType);

    }

    // Internal valible
    var initPopoverAlertFeed = function (feedItem) {
        var feedTemplate = (!feedItem.feedTemplate || feedItem.feedTemplate == "") ? buildDefaultHtmlFeedItem() : feedItem.feedTemplate;

        initPopover({
            title: feedItem.title,
            content: function () {
                //return $("#popover-content").html();
            },
            id: feedItem.id,
            getUri: feedItem.getMsgFeedUri,
            updateUri: feedItem.updateMsgStatusUri,
            deleteUri: feedItem.deleteGroupMsgUri,
            feedItemHtml: feedTemplate,
            timer: feedItem.alertTime,
            customCssClass: "alert",
            closeElm: alertComponentObj.CloseElm,
            clearElm: alertComponentObj.ClearElm,
            markallElm: alertComponentObj.MarkallElm,
            filterElm: alertComponentObj.ShowFilterElm,
            sortElm: alertComponentObj.SortElm,
        });
    }

    var showAlert = function (serverUri, updateUri, alertTimer, alertTimeClose) {
        //Alert option obj
        alertOption = {
            getUri: serverUri,
            updateUri: updateUri,
            alertTime: alertTimeClose * 1000,
            zIndex: '999',
            CloseText: ActionContent.CloseText,
            ViewText: ActionContent.ViewText,
            DismissText: ActionContent.DismissText,
            PendingStatus: Status.PENDING,
        };

        var alertModule = new AlertModule(alertOption);
        alertModule.buildAlertMsg();
    }

    var initActionBarToggle = function () {
        var $actionbar = $("#actionBar").length > 0 ? $("#actionBar") : $(".action-bar-container");
        var $actionBarTitle = $("#actionBarTitle").length > 0 ? $("#actionBarTitle") : $(".action-bar-title");

        var $col = $actionbar.find("[data-col]");
        var $exp = $actionbar.find("[data-exp]");

        $actionBarTitle.click(function () {
            if (!$actionbar.hasClass("open")) {
                $actionbar.addClass("open");
            }
            reSetAlertBottom();
        });

        $col.click(function (e) {
            e.preventDefault();
            if ($actionbar.hasClass("open")) {
                $actionbar.removeClass("open");
            }
            reSetAlertBottom();
        });

        $exp.click(function (e) {
            e.preventDefault();
            if (!$actionbar.hasClass("open")) {
                $actionbar.addClass("open");
            }
            reSetAlertBottom();
        });

        var actionStatus = getCookie('actionBarOpen');
        if (actionStatus == "true") {
            $('.action-bar-container').addClass('open');
        } else {
            $('.action-bar-container').removeClass('open');
        }

    }

    //Default 
    var SortValue = SortDes.DESC;
    var FilterStatusValue = FilterStatus.All;
    var FilterSourceAppValue = FilterSourceApp.All;

    var feedheight = 500;
    var isRequesting = false;
    var isGroupDeleteding = false;
    var isItemUpdateding = false;
    var alertFeedSize = 15;
    var curMaxFeedItemId = 0;
    var curFeedUnresdTotal = 0;
    var currentFeedRowTotal = 0;
    var currentAlertFeedIndex = 0;
    var curMaxFeedItemIdAllSource = 0;
    var totalAllSource = 0;
    var totalPendingAllSource = 0;
    // Load more item
    var alertFeedRowTotal = 0;
    var alertMaxIndexID = 0;

    var lastAlertFeedIndex = 0;
    var FilterAction = false;

    var initPopover = function (options) {
        var options = $.extend({
            elm: $('[data-toggle="popover"]'),
            html: true,
            id: "",
            title: "Popover",
            feedItemHtml: "",
            getUri: "",
            updateUri: "",
            deleteUri: "",
            timer: 5000,
            customCssClass: "fixed",
            content: "",
            closeElm: "",
            clearElm: ""
        }, options);

        var itemId = "js-content-" + options.id;
        $('#js-popover-content').append('<div id=' + itemId + '></div>');
        buildActionItemFeed(options.feedItemHtml, options.getUri, itemId);

        var feedHeader = "<div class='popover-header'>" +
                                 "<div class='display-flex align-center'><H3 class='alert-title'>" + options.title + "</H3><span id='btnNewFeed' class='badge'>[new]</span><span id='btnUnread' class='badge'>" + ActionContent.UnRead + "</span></div>" +
                                 "<div class='display-flex align-center'><a id='btnMarkAllRead' class='action-link'>" + ActionContent.MarkAllRead + "</a><a id='btnClearFeed' class='action-link action-link-danger'>" + ActionContent.ClearAll + "</a></div>" +
                             "</div>";

        var feedFooter = "<div class='popover-footer'> <span id='lblTotal' class='alert-num'>Alert</span> <span id='btnCloseFeed' class='btn btn-secondary'>Close</span></div>";

        feedHeader += buildAlertFilterHtml();

        var actionItem = $('#' + options.id).find('[data-toggle="popover"]');
        var $elm = actionItem.popover({
            html: options.html,
            title: options.title,
            sanitize: false,
            content: function () {
                // return buildActionItemFeed(options.feedItemHtml, options.getUri);    
                var feedHtml = $('#' + itemId).html();
                if (feedHtml == '') {
                    feedHtml = "<div id='btnFeedLoading' class='" + alertComponentObj.Loading + "'></div>";
                }
                return feedHtml;
            },
            template: "<div class='popoverAlert popover " + options.customCssClass + "'><div class='arrow'></div>" + feedHeader + "<div class='popover-content'></div>" + feedFooter + "</div>"
        });


        $('#' + options.id + 'bubble').removeClass("bubble");
        //$("#lkCloseFeed").hide();

        //Update runtime 
        setInterval(function () {
            if (options.getUri && !isRequesting && !isGroupDeleteding && !isItemUpdateding) {
                buildActionItemFeed(options.feedItemHtml, options.getUri, itemId);
            }
        }, 1000 * options.timer);

        $elm.on("shown.bs.popover", function () {
            $(alertComponentObj.NewFeedElm).css("display", "none");
            $("#div-alert").addClass("js-feedopen");
            $("body").addClass("overflow-hidden");
            UpdateSourceAppUI();

            //$("#lkCloseFeed").show();
            //$("#lkOpenFeed").hide();
            //showDismissAll();
            var $popover = $(this).data('bs.popover').tip(),
                $closeButton = $popover.find(options.closeElm),
                $popoverContainer = $('.popover');

            // Sort
            $popover.find(options.sortElm).unbind("click");
            $popover.find(options.sortElm).click(function (e) {
                if ($(this).hasClass("active")) {
                    $(this).removeClass("active");
                    SortValue = SortDes.DESC;
                } else {
                    $(this).addClass("active");
                    SortValue = SortDes.ASC;
                }

                onSortAlert();
                e.stopPropagation();
            });

            // Filter
            $popover.find(options.filterElm).unbind("click");
            $popover.find(options.filterElm).click(function (e) {
                var objFilter = $(this).parent();
                if ($(objFilter).hasClass("open-dropdown")) {
                    $(objFilter).removeClass("open-dropdown");
                    onFilterAlert();
                } else {
                    $(objFilter).addClass("open-dropdown");
                }
                e.stopPropagation();
            });

            // Filter Source event
            $popover.find('.js-sourceitem').unbind("click");
            $popover.find('.js-sourceitem').click(function () {
                var id = $(this).attr("id");
                var allItem = $popover.find('.js-sourceall');
                if (id == "-1") {
                    if ($(this).hasClass("active")) {
                        $(this).removeClass("active");
                        $popover.find('.js-sourceitem').removeClass("active");
                    } else {
                        $(this).addClass("active");
                        $popover.find('.js-sourceitem').addClass("active");
                    }
                } else {
                    if ($(this).hasClass("active")) {
                        $(this).removeClass("active");
                        $(allItem).removeClass("active");
                    } else {
                        $(this).addClass("active");

                        // Set all
                        if (!$(allItem).hasClass("active") && $("#ulSources").find('.js-sourceitem:not(.js-sourceall):not(.active)').length == 0) {
                            $(allItem).addClass("active");
                        }
                    }
                }

                FilterAction = true;
            });

            // Filter Status event
            $popover.find('.js-statusitem').unbind("click");
            $popover.find('.js-statusitem').click(function (e) {
                var id = $(this).attr("id");

                if (!$(this).hasClass("active")) {
                    $popover.find('.js-statusitem').removeClass("active");
                    $(this).addClass("active");
                    FilterStatusValue = id;
                }

                FilterAction = true;
            });

            // Close filter           
            $(document).click(function (e) {
                var objTarget = e.target;
                var objFilterContent = $popover.find('.dropdown-group-content');

                if (!$(objFilterContent).find(objTarget).length) {
                    var objFilter = $('.container-dropdown-group');
                    if ($(objFilter).hasClass("open-dropdown")) {
                        $(objFilter).removeClass("open-dropdown");
                        onFilterAlert();
                    }
                }
            });

            // Close alert feed
            $closeButton.unbind("click");
            $closeButton.click(function () {
                $elm.popover("hide");
            });

            // Delete all alert feed by fielter
            $popover.find(options.clearElm).unbind("click");
            $popover.find(options.clearElm).click(function () {
                // To do
                var alertIDs = "";
                setLoading(true);
                closeFeedItem(options.updateUri, alertIDs, Status.DELETE, alertActions.DeleteAll);
            });

            // Mark all read
            $popover.find(options.markallElm).unbind("click");
            $popover.find(options.markallElm).click(function () {
                var alertIDs = "";
                setLoading(true);
                closeFeedItem(options.updateUri, alertIDs, Status.READ, alertActions.MarkAllRead);
                //removeAlert(alertIDs, false);               
            });

            $popover.find(alertComponentObj.NewFeedElm).unbind("click");
            $popover.find(alertComponentObj.NewFeedElm).click(function () {
                $(alertComponentObj.NewFeedElm).css("display", "none");
                reLoadAlertFeed();

                //$popover.find('.popover-content').html($('#' + itemId).html());
                //$popover.find('.popover-content .content').scrollTop(0);
            });

            // Alert Item Event 
            feedItemEvent($popover);

            // Alert layout Event 
            $popover.find('.popover-content .content').unbind("scroll");
            $popover.find('.popover-content .content').scroll(function () {
                if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) {
                    loadMoreItemFeed(options.feedItemHtml, options.getUri);
                }

                if ($(this).scrollTop() == 0) {
                    $(alertComponentObj.NewFeedElm).css("display", "none");
                }

            });

            $(alertComponentObj.NewFeedElm).css("display", "none");

            // Set Total 
            setTotal(AlertActionType.TOTAL);
            setTotal(AlertActionType.UNREAD);

            showHideMarkAll();
            showHideClearAll();


            //$('#' + options.id + 'bubble').removeClass("bubble");

            currentAlertFeedIndex = 2;
            feedheight = $popover.find('.popover-content .content').height();

            //reset position for popover
            $popoverContainer.addClass('alert-fixed');
        });

        $elm.on("hidden.bs.popover", function (e) {
            if ($(e.target).data('bs.popover').inState) {
                $(e.target).data('bs.popover').inState.click = false;
            }
            $("#div-alert").removeClass("js-feedopen");
            $("body").removeClass("overflow-hidden");

            //$("#lkCloseFeed").hide();
            //$("#lkOpenFeed").show();            
            //showDismissAll();

            //<fix Bug #33274>
            var divPopover = $('body').find('.popover.alert.alert-fixed');
            if (divPopover.length > 0) {
                divPopover.remove();
            }
            //</fix Bug #33274>
        });

        $("#lkCloseFeed").unbind("click");
        $("#lkCloseFeed").click(function (e) {
            $elm.popover("hide");
        });

        $("#lkOpenFeed").unbind("click");
        $("#lkOpenFeed").click(function (e) {
            $elm.popover("show");
        });

        $("#lkDismissAll").unbind("click");
        $("#lkDismissAll").click(function (e) {
            closeFeedItem(alertObj.updateMsgStatusUri, "", Status.VIEW, alertActions.DismissAll);
        });

        $("#div-alert").unbind("hover");
        $("#div-alert").hover(function (e) {
            showDismissAll();
        });

    }

    var disabledToolbar = function (disabled) {
        if (disabled) {
            $("#lkSortAlert").addClass("disabled");
            $(alertComponentObj.MarkallElm).addClass("disabled");
            $(alertComponentObj.ClearElm).addClass("disabled");
        } else {
            $("#lkSortAlert").removeClass("disabled");
            $(alertComponentObj.MarkallElm).removeClass("disabled");
            $(alertComponentObj.ClearElm).removeClass("disabled");
        }
    }

    var setLoading = function (isLoading) {
        var loadingHtml = "<div id='btnAlertLoading' class='" + alertComponentObj.Loading + "'></div>";
        if (isLoading) {
            if ($(".popover-content").find("#btnAlertLoading").length) {
                $(".popover-content").find("#btnAlertLoading").addClass(alertComponentObj.Loading);
            } else {
                $(".popover-content").append(loadingHtml);
            }
            //$(".popover-content").addClass("overflow-hidden");           
        } else {
            $(".popover-content").find("#btnAlertLoading").removeClass(alertComponentObj.Loading);
            //$(".popover-content").removeClass("overflow-hidden");            
        }

        if ($('.popover-content .content').height() > ($(window).height() - 240)) {
            $('.popover-content .content').addClass('content-height');
        }
    }

    var feedItemEvent = function (alertPopFeedObj) {
        var actionId = alertObj.id;
        //Event 
        var feedItemHtml = buildDefaultHtmlFeedItem();
        var serverUpdateUri = alertObj.updateMsgStatusUri;
        var serverDeleteUri = alertObj.deleteGroupMsgUri;
        var serverGetUri = alertObj.getMsgFeedUri;
        var itemId = "js-content-" + alertObj.id;

        // Alert layout Event 
        $(alertPopFeedObj).find('.popover-content .content').unbind("scroll");
        $(alertPopFeedObj).find('.popover-content .content').scroll(function () {
            if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) {
                loadMoreItemFeed(feedItemHtml, serverGetUri);
            }

            if ($(this).scrollTop() == 0) {
                $(alertComponentObj.NewFeedElm).css("display", "none");
            }
        });

        // View
        $(alertPopFeedObj).find('.js-feed-action-item-view').unbind("click");
        $(alertPopFeedObj).find('.js-feed-action-item-view').click(function () {
            var objTemp = $(this).parent().parent().parent();
            var notificationLink = $(objTemp).attr('linkto');
            var notificationId = $(objTemp).attr('id');

            // Update status as read 
            closeFeedItem(serverUpdateUri, notificationId, Status.READ, alertActions.View);
            openAlertLink(notificationLink);

            // Update UI
            if (checkitemPending($(objTemp).attr('status'))) {
                curFeedUnresdTotal = curFeedUnresdTotal - 1;
                totalPendingAllSource = totalPendingAllSource - 1;
                setTotal(AlertActionType.UNREAD);

                // Remove UI            
                $(objTemp).find("#" + notificationId + "-mark").remove();
                $(objTemp).find("#" + notificationId + "-bubble").remove();
            }

            // Filter logic- if only show Unread alert then remove this item
            if (FilterStatusValue == FilterStatus.UNREAD) {
                $(objTemp).parent().remove();

                currentFeedRowTotal = currentFeedRowTotal - 1;
                setTotal(AlertActionType.TOTAL);

                var items = $('.popover .js-feed-action-item').length;
                if (items == 0) {
                    setNoItem();
                }

                var itemGroup = $('.popover .js-' + notificationGroup).length;
                if (itemGroup == 0) {
                    $('.popover .js-item-group-' + notificationGroup).remove();
                }

                deleteAlertBE(alertObj.id, notificationId);

                // Load more item 
                canLoadMoreFeed(feedItemHtml, serverGetUri);
            }


        });

        // Mark read
        $(alertPopFeedObj).find('.js-feed-action-item-mark').unbind("click");
        $(alertPopFeedObj).find('.js-feed-action-item-mark').click(function () {
            var objTemp = $(this).parent().parent().parent().parent();
            var notificationId = objTemp.attr('id');
            var notificationGroup = objTemp.attr('itemgroup');

            // Update status as read 
            closeFeedItem(serverUpdateUri, notificationId, Status.READ, alertActions.MarkRead);

            // Remove UI            
            $(this).remove();
            $(objTemp).find("#" + notificationId + "-bubble").remove();

            curFeedUnresdTotal = curFeedUnresdTotal - 1;
            totalPendingAllSource = totalPendingAllSource - 1;
            setTotal(AlertActionType.UNREAD);

            // Filter logic- if only show Unread alert then remove this item
            if (FilterStatusValue == FilterStatus.UNREAD) {
                $(objTemp).parent().remove();

                currentFeedRowTotal = currentFeedRowTotal - 1;
                setTotal(AlertActionType.TOTAL);

                var items = $('.popover .js-feed-action-item').length;
                if (items == 0) {
                    setNoItem();
                }

                var itemGroup = $('.popover .js-' + notificationGroup).length;
                if (itemGroup == 0) {
                    $('.popover .js-item-group-' + notificationGroup).remove();
                }

                deleteAlertBE(alertObj.id, notificationId);

                // Load more item 
                canLoadMoreFeed(feedItemHtml, serverGetUri);
            }

        });

        // Clear
        $(alertPopFeedObj).find('.js-feed-action-item-delete').unbind("click");
        $(alertPopFeedObj).find('.js-feed-action-item-delete').click(function () {
            $(this).parent().parent().parent().parent().fadeOut(200, function () {
                var notificationId = $(this).attr('id');
                var notificationGroup = $(this).attr('itemgroup');
                closeFeedItem(serverUpdateUri, notificationId, Status.DELETE, alertActions.Delete);

                // If this item is pending
                if (checkitemPending($(this).attr('status'))) {
                    curFeedUnresdTotal = curFeedUnresdTotal - 1;
                    totalPendingAllSource = totalPendingAllSource - 1;
                    setTotal(AlertActionType.UNREAD);
                }

                currentFeedRowTotal = currentFeedRowTotal - 1;
                totalAllSource = totalAllSource - 1;
                setTotal(AlertActionType.TOTAL);

                $(this).parent().remove();

                var items = $('.popover .js-feed-action-item').length;
                if (items == 0) {
                    setNoItem();
                }

                var itemGroup = $('.popover .js-' + notificationGroup).length;
                if (itemGroup == 0) {
                    $('.popover .js-item-group-' + notificationGroup).remove();
                }

                // Load more item 
                canLoadMoreFeed(feedItemHtml, serverGetUri);
                deleteAlertBE(actionId, notificationId);
            });
        });

        // Group
        $(alertPopFeedObj).find('.js-group').unbind("click");
        $(alertPopFeedObj).find('.js-group').click(function () {
            var objTemp = $(this).parent();
            var fDate = $(objTemp).attr('fdate');
            var tDate = $(objTemp).attr('tdate');
            var iDs = "";
            closeFeedItems(serverDeleteUri, fDate, tDate, Status.DELETE, iDs, objTemp);

            //    // Update BE

            //    //buildActionItemFeed(feedItemHtml, serverGetUri, itemId);
            //    //removeAlert("", false);
            //    //deleteAlertBE(actionId, iDs);

        });

        if ($('.popover-content .content').height() > ($(window).height() - 240)) {
            $('.popover-content .content').addClass('content-height');
        }

    }

    var setTotal = function (type) {
        if (type == AlertActionType.UNREAD) {
            if (Number(totalPendingAllSource) > 0) {
                $(alertComponentObj.UnreadIndicator).show();
                var DislayText = totalPendingAllSource + " Unread";
                $(alertComponentObj.UnreadIndicator).html(DislayText);
            } else {
                $(alertComponentObj.UnreadIndicator).hide();
                $(alertComponentObj.UnreadIndicator).html('');
            }

            showHideMarkAll();
        }

        if (type == AlertActionType.TOTAL) {
            var DislayText = totalAllSource + " Alerts";
            if (Number(totalAllSource) == 1) {
                DislayText = totalAllSource + " Alert";
            }

            $(alertComponentObj.TotalIndicator).html(DislayText);

            showHideClearAll();
        }
    }

    var showHideMarkAll = function () {
        var obj = $(alertComponentObj.MarkallElm);
        if (Number(curFeedUnresdTotal) > 0) {
            obj.removeClass("disabled");
        } else {
            obj.addClass("disabled");
            $(alertComponentObj.UnreadIndicator).hide();
            $(alertComponentObj.UnreadIndicator).html('');
            $(alertComponentObj.NewFeedElm).css("display", "none");
        }
    }

    var showHideClearAll = function () {
        var obj = $(alertComponentObj.ClearElm);
        if (Number(totalAllSource) > 0) {
            obj.show();
            $(alertComponentObj.MarkallElm).show();
            $(".popup-toolbar").show();
            if (Number(currentFeedRowTotal) > 0) {
                obj.removeClass("disabled");
                disabledToolbar(false);
            } else {
                obj.addClass("disabled");
                curFeedUnresdTotal = 0;
                showHideMarkAll();
                disabledToolbar(true);
            }

        } else {
            obj.hide();
            $(alertComponentObj.MarkallElm).hide();
            curFeedUnresdTotal = 0;
            showHideMarkAll();
            $(".popup-toolbar").hide();
        }
    }

    var onSortAlert = function () {
        setLoading(true);
        reLoadAlertFeed();
    }

    var onFilterAlert = function () {
        if (FilterStatusValue == "5") {
            FilterStatusValue = FilterStatus.UNREAD;
        }

        if ($('.js-sourceall').hasClass("active")) {
            FilterSourceAppValue = FilterSourceApp.All;
        } else {
            var tempObj = $('.js-sourceitem');
            if (tempObj.length > 0) {
                FilterSourceAppValue = "";
                for (var i = 0; i < tempObj.length; i++) {
                    if ($(tempObj[i]).hasClass("active")) {
                        if (FilterSourceAppValue != '') FilterSourceAppValue += ",";
                        FilterSourceAppValue += $(tempObj[i]).attr("id");
                    }
                }
            }
            else {
                FilterSourceAppValue = "";
            }
        }

        if (FilterAction) {
            setLoading(true);
            FilterAction = false;
            dislayFilter();
            reLoadAlertFeed();
        }
    }

    var dislayFilter = function () {
        var filtertextHtml = "";
        if (FilterSourceAppValue == "-1") {
            filtertextHtml += "<strong>All</strong> sources";
        } else {
            var sources = FilterSourceAppValue != "" ? FilterSourceAppValue.split(',').length : 0;
            var sourcetext = (sources == 0 || sources == 1) ? " source" : " sources";
            filtertextHtml += "<strong>" + sources.toString() + "</strong>" + sourcetext;
        }

        // status       
        var filterStatus = FilterStatusValue == "5,8" ? "5" : FilterStatusValue;
        for (var i = 0; i < statuApps.length; i++) {
            if (statuApps[i].ID == filterStatus) {
                filtertextHtml += " <span class='text-light-grey'>|</span> " + "<strong>" + statuApps[i].Name + "</strong> status";
                break;
            }
        }

        $(alertComponentObj.FilterText).html(filtertextHtml);
        return filtertextHtml;
    }

    // Call from ext
    var onUpdateFeedItemView = function (alertId) {
        var bsPopup = $('#' + alertObj.id).find('[data-toggle="popover"]').data('bs.popover');
        if (bsPopup) {
            var alertFeed = bsPopup.tip();
            var itemObj = $(alertFeed).find("#" + alertId);

            if (itemObj.length == 1) {
                if (checkitemPending($(itemObj).attr('status'))) {
                    // Remove UI
                    $(itemObj).find("#" + alertId + "-mark").remove();
                    $(itemObj).find("#" + alertId + "-bubble").remove();

                    curFeedUnresdTotal = curFeedUnresdTotal - 1;
                    totalPendingAllSource = totalPendingAllSource - 1;
                    setTotal(AlertActionType.UNREAD);

                    // Filter logic- if only show Unread alert then remove this item
                    if (FilterStatusValue == FilterStatus.UNREAD) {
                        $(itemObj).parent().remove();

                        currentFeedRowTotal = currentFeedRowTotal - 1;
                        totalAllSource = totalAllSource - 1;
                        setTotal(AlertActionType.TOTAL);

                        var notificationGroup = itemObj.attr('itemgroup');
                        var itemGroup = $('.popover .js-' + notificationGroup).length;
                        if (itemGroup == 0) {
                            $('.popover .js-item-group-' + notificationGroup).remove();
                        }

                        var items = $('.popover .js-feed-action-item').length;
                        if (items == 0) {
                            setNoItem();
                        }

                        deleteAlertBE(alertObj.id, alertId);
                    }

                }
            }
        }
    }

    var checkitemPending = function (status) {
        return status == Status.PENDING || status == Status.VIEW;
    }

    var removeAlert = function (notificationId, isCheckPersistant) {
        var alertModule = new AlertModule(alertOption);
        if (notificationId != "") {
            alertModule.canRemoveAlertMsg(notificationId, isCheckPersistant);
        } else {
            alertModule.reBuildAlertMsg();
        }
    }

    var buildActionItemFeed = function (feedHtmlTemplate, serverUri, itemId, groupObj) {
        if (!feedHtmlTemplate) feedHtmlTemplate = buildDefaultHtmlFeedItem();
        var tempObj = $('#' + itemId);
        if (serverUri) {
            var datas = {
                pageNo: 0, pageSize: alertFeedSize, isPaging: true, maxIndex: -1,
                sourceIDs: FilterSourceAppValue, statusIDs: FilterStatusValue, sDescription: SortValue
            };
            var serverType = "POST";
            isRequesting = true;
            var secparamobj = "";
            if (typeof (getSecAlertParams) != "undefined") {
                secparamobj = getSecAlertParams;
            }
            setTimeout(function () {
                alertAjax(serverUri, datas, function (data) {
                    isRequesting = false;
                    if (data) {
                        var datas = data.d ? data.d : data;
                        tempObj.html('<div class="content"></div>');
                        if (datas.length > 0) {
                            tempObj.find(".content").append(BuildAlertGroup(datas, feedHtmlTemplate));
                            tempObj.find(".content").removeClass("position-static");
                            if (!isGroupDeleteding && !isItemUpdateding) {
                                checkAlertPending(datas, false, curMaxFeedItemId);
                            }
                            currentFeedRowTotal = datas[0].TotalRows;
                            curMaxFeedItemId = datas[0].NotificationAlertMsgID;
                            curFeedUnresdTotal = datas[0].TotalRowsPending;
                            totalAllSource = datas[0].TotalRowsAllSource;
                            totalPendingAllSource = datas[0].TotalRowsPendingAllSource;
                        } else {
                            currentFeedRowTotal = 0;
                            curFeedUnresdTotal = 0;
                            tempObj.find(".content").append(buildAlertFeedNoItemHtml());
                            tempObj.find(".content").addClass("position-static");
                            if (FilterSourceAppValue == FilterSourceApp.All && FilterStatusValue == FilterStatus.All) {
                                $('#' + alertObj.id + 'bubble').removeClass("bubble");
                                totalAllSource = 0;
                                totalPendingAllSource = 0;
                            } else {
                                checkDataPendingAllSource();
                            }
                        }

                        if (groupObj) {
                            //Remove group
                            var groupname = $(groupObj).attr('groupname');
                            var groups = $('.popover .js-' + groupname);
                            for (var i = 0; i < groups.length; i++) {
                                $(groups[i]).parent().remove();
                            }

                            $(groupObj).remove();
                            isGroupDeleteding = false;
                            setLoading(false);
                            canLoadMoreFeed(buildDefaultHtmlFeedItem(), alertObj.getMsgFeedUri, true);
                        }

                        if (!isGroupDeleteding && !isItemUpdateding) {
                            setTotal(AlertActionType.TOTAL);
                            setTotal(AlertActionType.UNREAD);
                        }

                        if (currentFeedRowTotal == 0) {
                            setNoItem();
                        }

                        if ($('#' + alertObj.id).find('[data-toggle="popover"]').data('bs.popover')) {
                            var alertFeed = $('#' + alertObj.id).find('[data-toggle="popover"]').data('bs.popover').tip();

                            if (alertFeed.hasClass('in') && $(alertFeed).find('.popover-content').find("#btnFeedLoading").length) {
                                $(alertFeed).find('.popover-content').html($('#' + itemId).html());
                                $(alertFeed).find('.popover-content').scrollTop(0);
                                feedItemEvent(alertFeed);
                            }
                        }
                    }

                    return tempObj.html();
                }, serverType, secparamobj);
            }, 0);
        }

        return tempObj.html();
    }

    var loadMoreItemFeed = function (feedHtmlTemplate, serverUri) {
        var urlServer = serverUri;
        if (serverUri) {
            if (currentAlertFeedIndex == 2) {
                // Set lated index for load more
                currentAlertFeedIndex = lastAlertFeedIndex;
                alertFeedRowTotal = currentFeedRowTotal;
            }

            var items = $('.popover .js-feed-action-item').length;
            if (items >= alertFeedRowTotal) {
                if (items == 0) {
                    setNoItem();
                }
                return;
            }

            var datas = {
                pageNo: currentAlertFeedIndex, pageSize: alertFeedSize, isPaging: true, maxIndex: alertMaxIndexID,
                sourceIDs: FilterSourceAppValue, statusIDs: FilterStatusValue, sDescription: SortValue
            };

            var serverType = "POST";
            var secparamobj = "";
            if (typeof (getSecAlertParams) != "undefined") {
                secparamobj = getSecAlertParams;
            }

            setLoading(true);
            alertAjax(serverUri, datas, function (data) {
                if (data) {
                    var datas = data.d ? data.d : data;
                    if (datas.length > 0) {
                        for (var i = 0; i < datas.length; i++) {
                            buildRuntimeMoreAlertFeed(datas[i]);
                        }
                        currentAlertFeedIndex = datas[datas.length - 1].NotificationAlertMsgID;

                    }
                }
                setLoading(false);
            }, serverType, secparamobj);
        }
    }

    var closeFeedItem = function (serverUri, notificationId, statusId, action) {
        var datas = { alertId: notificationId, status: statusId, sourceIDs: FilterSourceAppValue };
        var serverType = "POST";
        var secparamobj = (typeof (updateSecAlertParams) != "undefined") ? updateSecAlertParams : '';
        setLoading(true);
        isItemUpdateding = true;
        alertAjax(serverUri, datas, function (data) {
            isItemUpdateding = false;
            if (statusId == Status.DELETE || statusId == Status.READ) {
                if (action == alertActions.MarkAllRead || action == alertActions.DeleteAll) {
                    reLoadAlertFeed(action);
                } else {
                    setLoading(false);
                }
                removeAlert(notificationId);
            }

            if (action == alertActions.DismissAll) {
                removeAlert(notificationId);
            }

        }, serverType, secparamobj);
    }

    var closeFeedItems = function (serverUri, fDate, tDate, statusId, notificationId, groupObj) {
        //if (notificationId == '') return;
        isGroupDeleteding = true;
        setLoading(true);
        var datas = { fDate: fDate, tDate: tDate, sourceIDs: FilterSourceAppValue };
        var serverType = "POST";
        var secparamobj = (typeof (deleteSecAlertParams) != "undefined") ? deleteSecAlertParams : '';
        alertAjax(serverUri, datas, function (data) {
            if (statusId == Status.DELETE) {
                var feedItemHtml = buildDefaultHtmlFeedItem();
                var serverGetUri = alertObj.getMsgFeedUri;
                var itemId = "js-content-" + alertObj.id;

                buildActionItemFeed(feedItemHtml, serverGetUri, itemId, groupObj);
                removeAlert("", false);
            }
        }, serverType, secparamobj);
    }

    var deleteAlertBE = function (actionId, itemId) {
        var actionItem = $('#js-content-' + actionId);
        if (actionItem && itemId) {
            var feedIdList = itemId.split(',');
            var itemGroupname = "";
            for (var j = 0; j < feedIdList.length; j++) {
                var item = $(actionItem).find('[id=' + feedIdList[j] + ']');
                if (!itemGroupname) {
                    itemGroupname = $(item).attr('itemgroup');
                }
                $(item).parent().remove();
            }

            var itemGroups = $(actionItem).find('.js-' + itemGroupname).length;
            if (itemGroups == 0) {
                $(actionItem).find('.popover .js-item-group-' + itemGroupname).remove();
            }
        }
    }

    var setNoItem = function () {
        var content = $('.popoverAlert .popover-content').find(".content");
        if (content) {
            if (!content.find(".js-item-start").length) {
                var noItemHtml = buildAlertFeedNoItemHtml();
                $(content).append(noItemHtml);
                $(content).addClass("position-static");
            }
        }
    }

    // Build Run time Alert Feed . When alert feed are showing
    var buildRuntimeMoreAlertFeed = function (alertitem) {
        if (!alertObj) return;
        var actionId = alertObj.id;

        if (!$('#' + actionId).find('[data-toggle="popover"]').data('bs.popover')) return;

        var alertFeed = $('#' + actionId).find('[data-toggle="popover"]').data('bs.popover').tip();
        if (alertFeed.hasClass('in')) {
            var itemhtml = BuildRunTimeAlertItem(alertitem);
            if (itemhtml) {
                var notificationGroup = $(itemhtml).find('.js-feed-action-item').attr('itemgroup');
                var curGroup = $('.popover .js-item-group-' + notificationGroup).length;

                if (curGroup == 0) {
                    //create new group on UI 
                    var groupHtml = '<div groupname="{GroupName}" fdate="{FDate}" tdate="{TDate}" class="js-item-group-{Groupclass} title-group-alert"><span class="js-group">{Text}</span></div>';
                    groupHtml = groupHtml.replace("{Text}", GetGroupContent(notificationGroup)).replace("{Groupclass}", notificationGroup).replace("{GroupName}", notificationGroup)
                        .replace("{FDate}", GetGroupFDate(notificationGroup))
                        .replace("{TDate}", GetGroupTDate(notificationGroup));
                    $('.popover .popover-content .content').append(groupHtml);
                }

                $('.popover .popover-content .content').append(itemhtml);

                //Event 
                feedItemEvent(alertFeed);
            }
        }
    }

    var checkAlertPending = function (datas, isReload, maxItem) {
        if (!alertObj) return;
        var actionId = alertObj.id;

        if (datas.length > 0) {
            var alertPending = false;
            var alertPendingAllSource = datas[0].TotalRowsPendingAllSource;
            var alertIDLasted = datas[0].NotificationAlertMsgID_Max;

            // Set unread indicator 
            if (alertPendingAllSource > 0) {
                $('#' + actionId + 'bubble').addClass("bubble");
            } else {
                $('#' + alertObj.id + 'bubble').removeClass("bubble");
            }

            // Can load alert toast
            if (Number(alertIDLasted) > Number(curMaxFeedItemIdAllSource)) {
                var alertModule = new AlertModule(alertOption);
                alertModule.buildAlertMsg();
                curMaxFeedItemIdAllSource = alertIDLasted;
            }

            // Current source app pending item
            for (var i = 0; i < datas.length; i++) {
                var itemValue = Number(datas[i].NotificationAlertMsgID);
                if (checkitemPending(datas[i].StatusID) && itemValue > maxItem) {
                    alertPending = true;
                    break;
                }
            }

            if (alertPending) {
                var currFeed = $('#' + actionId).find('[data-toggle="popover"]').data('bs.popover');
                if (currFeed) {
                    var alertFeed = currFeed.tip();
                    if (alertFeed.hasClass('in')) {
                        if (!isReload) {
                            var contentObj = alertFeed.find('.popover-content .content');
                            var scrollTop = contentObj.scrollTop();
                            //var v1 = contentObj[0].scrollHeight;
                            //var v2 = contentObj.innerHeight();
                            if (scrollTop == 0) {
                                var itemId = "js-content-" + alertObj.id;
                                $('.popoverAlert .popover-content').html($('#' + itemId).html());
                                feedItemEvent(alertFeed);
                                lastAlertFeedIndex = datas[datas.length - 1].NotificationAlertMsgID;
                            } else {
                                if (SortValue == SortDes.DESC) {
                                    //$('#btnNewFeed').css("display", "inline");
                                }
                            }
                        }
                    }
                }

                return;
            }

            lastAlertFeedIndex = datas[datas.length - 1].NotificationAlertMsgID;

        }
    }

    var checkDataPendingAllSource = function () {
        var serverUri = alertObj.getMsgFeedUri;
        var datas = {
            pageNo: 0, pageSize: 1, isPaging: true, maxIndex: -1,
            sourceIDs: FilterSourceApp.All, statusIDs: FilterStatus.All, sDescription: "desc"
        };
        var serverType = "POST";
        isRequesting = true;
        var secparamobj = "";
        if (typeof (getSecAlertParams) != "undefined") {
            secparamobj = getSecAlertParams;
        }
        setTimeout(function () {
            alertAjax(serverUri, datas, function (data) {
                isRequesting = false;
                if (data) {
                    var datas = data.d ? data.d : data;
                    if (datas.length > 0) {
                        //currentFeedRowTotal = datas[0].TotalRows;                      
                        //curFeedUnresdTotal = datas[0].TotalRowsPending;
                        totalAllSource = datas[0].TotalRowsAllSource;
                        totalPendingAllSource = datas[0].TotalRowsPendingAllSource;

                        var alertPendingAllSource = datas[0].TotalRowsPendingAllSource;
                        var alertIDLasted = datas[0].NotificationAlertMsgID_Max;

                        // Set unread indicator 
                        if (alertPendingAllSource > 0) {
                            $('#' + alertObj.id + 'bubble').addClass("bubble");
                        } else {
                            $('#' + alertObj.id + 'bubble').removeClass("bubble");
                        }

                        // Can load alert toast
                        if (Number(alertIDLasted) > Number(curMaxFeedItemIdAllSource)) {
                            var alertModule = new AlertModule(alertOption);
                            alertModule.buildAlertMsg();
                            curMaxFeedItemIdAllSource = alertIDLasted;
                        }
                    } else {
                        totalAllSource = 0;
                        totalPendingAllSource = 0;
                        $('#' + alertObj.id + 'bubble').removeClass("bubble");
                    }
                }

                setTotal(AlertActionType.TOTAL);
                setTotal(AlertActionType.UNREAD);

            }, serverType, secparamobj);
        }, 0);
    }

    var canLoadMoreFeed = function (feedItemHtml, getUri, isGroup) {
        if (isGroup && currentFeedRowTotal > 0) {
            loadMoreItemFeed(feedItemHtml, getUri);
            return;
        }

        if ($('.popover-content .content')[0]) {
            var feedScrollheight = $('.popover-content .content')[0].scrollHeight;
            if (feedheight && feedScrollheight) {
                if (feedScrollheight < feedheight) {
                    loadMoreItemFeed(feedItemHtml, getUri);
                }
            }
        }
    }

    function reLoadAlertFeed(action) {
        if (!alertObj) return;
        var feedItemHtml = buildDefaultHtmlFeedItem();
        var serverGetUri = alertObj.getMsgFeedUri;
        var itemId = "js-content-" + alertObj.id;

        if (!feedItemHtml) feedItemHtml = buildDefaultHtmlFeedItem();
        var tempObj = $('#' + itemId);

        if (FilterSourceAppValue == "") {
            currentFeedRowTotal = 0;
            curFeedUnresdTotal = 0;

            tempObj.find(".content").append(buildAlertFeedNoItemHtml());
            tempObj.find(".content").addClass("position-static");

            if (FilterSourceAppValue == FilterSourceApp.All && FilterStatusValue == FilterStatusValue.All) {
                $('#' + alertObj.id + 'bubble').removeClass("bubble");
                totalAllSource = 0;
                totalPendingAllSource = 0;
            } else {
                checkDataPendingAllSource();
            }


            if (!$('#' + alertObj.id).find('[data-toggle="popover"]').data('bs.popover')) return;
            var alertFeed = $('#' + alertObj.id).find('[data-toggle="popover"]').data('bs.popover').tip();

            if (alertFeed.hasClass('in')) {
                setTotal(AlertActionType.TOTAL);
                setTotal(AlertActionType.UNREAD);
                $(alertFeed).find('.popover-content').html($('#' + itemId).html());
                $(alertFeed).find('.popover-content').scrollTop(0);
                feedItemEvent(alertFeed);
            }
        }
        else if (serverGetUri) {
            var datas = {
                pageNo: 0, pageSize: alertFeedSize, isPaging: true, maxIndex: -1,
                sourceIDs: FilterSourceAppValue, statusIDs: FilterStatusValue, sDescription: SortValue
            };
            var serverType = "POST";
            isRequesting = true;
            var secparamobj = "";
            if (typeof (getSecAlertParams) != "undefined") {
                secparamobj = getSecAlertParams;
            }
            setTimeout(function () {
                alertAjax(serverGetUri, datas, function (data) {
                    isRequesting = false;
                    setLoading(false);
                    if (data) {
                        var datas = data.d ? data.d : data;
                        tempObj.html('<div class="content"></div>');
                        if (datas.length > 0) {
                            tempObj.find(".content").append(BuildAlertGroup(datas, feedItemHtml));
                            tempObj.find(".content").removeClass("position-static");
                            checkAlertPending(datas, true, curMaxFeedItemId);
                            currentFeedRowTotal = datas[0].TotalRows;
                            curMaxFeedItemId = datas[0].NotificationAlertMsgID;
                            curFeedUnresdTotal = datas[0].TotalRowsPending;
                            lastAlertFeedIndex = datas[datas.length - 1].NotificationAlertMsgID;
                            totalAllSource = datas[0].TotalRowsAllSource;
                            totalPendingAllSource = datas[0].TotalRowsPendingAllSource;
                        } else {
                            currentFeedRowTotal = 0;
                            curFeedUnresdTotal = 0;

                            tempObj.find(".content").append(buildAlertFeedNoItemHtml());
                            tempObj.find(".content").addClass("position-static");

                            if (FilterSourceAppValue == FilterSourceApp.All && FilterStatusValue == FilterStatusValue.All) {
                                $('#' + alertObj.id + 'bubble').removeClass("bubble");
                                totalAllSource = 0;
                                totalPendingAllSource = 0;
                            } else {
                                checkDataPendingAllSource();
                            }
                        }
                    }

                    if (!$('#' + alertObj.id).find('[data-toggle="popover"]').data('bs.popover')) return;
                    var alertFeed = $('#' + alertObj.id).find('[data-toggle="popover"]').data('bs.popover').tip();

                    if (alertFeed.hasClass('in')) {
                        setTotal(AlertActionType.TOTAL);
                        setTotal(AlertActionType.UNREAD);
                        $(alertFeed).find('.popover-content').html($('#' + itemId).html());
                        $(alertFeed).find('.popover-content').scrollTop(0);
                        feedItemEvent(alertFeed);
                    }

                }, serverType, secparamobj);
            }, 0);
        }

        currentAlertFeedIndex = 2;
    }

    function UpdateSourceAppUI() {
        var urlServer = alertObj.getSourcesUri;
        var datas = {};
        var serverType = "GET";
        setTimeout(function () {
            alertAjax(urlServer, datas, function (data) {
                if (data) {
                    sourceApps = [];

                    if (data.d.length > 1 || data.d.length == 0) {
                        sourceApps.push(sourceAppsDefaultItem);
                    }

                    for (var i = 0; i < data.d.length; i++) {
                        var item = {};
                        item.ID = data.d[i].SourceID;
                        item.Name = data.d[i].SourceName;
                        sourceApps.push(item);
                    }

                    var itemHtml = '<li id="{ID}" class="{Class}">{Name}</li>';
                    var itemSources = '';
                    for (var i = 0; i < sourceApps.length; i++) {
                        var Itemclass = " active js-sourceitem";
                        if (sourceApps[i].ID == "-1" || sourceApps.length == 1) {
                            Itemclass += " active js-sourceall";
                        }

                        itemSources += itemHtml.replace("{ID}", sourceApps[i].ID).replace("{Name}", sourceApps[i].Name).replace("{Class}", Itemclass);
                    }

                    $("#ulSources").html(itemSources);

                    // Filter Source event
                    $("#ulSources").find('.js-sourceitem').unbind("click");
                    $("#ulSources").find('.js-sourceitem').click(function () {
                        var id = $(this).attr("id");
                        var allItem = $("#ulSources").find('.js-sourceall');
                        if (id == "-1") {
                            if ($(this).hasClass("active")) {
                                $(this).removeClass("active");
                                $("#ulSources").find('.js-sourceitem').removeClass("active");
                            } else {
                                $(this).addClass("active");
                                $("#ulSources").find('.js-sourceitem').addClass("active");
                            }
                        } else {
                            if ($(this).hasClass("active")) {
                                $(this).removeClass("active");
                                $(allItem).removeClass("active");
                            } else {
                                $(this).addClass("active");

                                // Set all
                                if (!$(allItem).hasClass("active") && $("#ulSources").find('.js-sourceitem:not(.js-sourceall):not(.active)').length == 0) {
                                    $(allItem).addClass("active");
                                }
                            }
                        }

                        FilterAction = true;
                    });
                }
            }, serverType);
        }, 0);
    }

    // Build Html

    function buildAlertFeedNoItemHtml() {
        var baseHtml = '<div class="js-item-start no-data-alert">';
        baseHtml += '<span class="icon-alert"></span><div class="caught-up">' + ActionContent.NoItemTitle + '</div><div>' + ActionContent.NoItem + '</div>';
        baseHtml += '</div>';

        return baseHtml;
    }

    function buildAlertFilterHtml() {
        var baseHtml = '<div class="popup-toolbar"><div class="container-dropdown-group">';
        baseHtml = baseHtml + '<div id="btnShowFilter">' + ActionContent.Show + ' <span id="lblFilterText">' + dislayFilter() + '</span><span class="icon-arrow-down"></span></div>';
        baseHtml = baseHtml + '<div class="dropdown-group-content">';
        baseHtml = baseHtml + '<div class="group">';
        baseHtml = baseHtml + '<div class="title-group">' + ActionContent.SourceText + '<i class="title-note">' + ActionContent.MultipleText + '</i></div>';
        baseHtml = baseHtml + '<ul id="ulSources">';
        baseHtml = baseHtml + '{FilterItems}';
        baseHtml = baseHtml + '</ul>';
        baseHtml = baseHtml + '</div>';
        baseHtml = baseHtml + '<div class="group">';
        baseHtml = baseHtml + '<div class="title-group">' + ActionContent.StatusText + '<i class="title-note">' + ActionContent.SingleText + '</i></div>';
        baseHtml = baseHtml + '<ul>';
        baseHtml = baseHtml + '{StatusItems}';
        baseHtml = baseHtml + '</ul>';
        baseHtml = baseHtml + '</div>';
        baseHtml = baseHtml + '</div>';
        baseHtml = baseHtml + '</div>';
        baseHtml = baseHtml + '<div class="display-flex"> Order by: <a id="lkSortAlert" class="action-link ml-xs"><span id="btnSortAlert" class="icon-sort"> ' +
                        '<span class="icon-sort-up"></span>' +
                        '<span class="icon-sort-down"></span>' +
                        '</span> </a></div>';
        //baseHtml = baseHtml + '<span id="btnAlertLoading" class=""></span>';
        baseHtml = baseHtml + '</div>';

        var itemHtml = '<li id="{ID}" class="{Class}">{Name}</li>';
        var itemSources = '';
        for (var i = 0; i < sourceApps.length; i++) {
            var Itemclass = " active js-sourceitem";
            if (sourceApps[i].ID == "-1") {
                Itemclass += " active js-sourceall";
            }

            itemSources += itemHtml.replace("{ID}", sourceApps[i].ID).replace("{Name}", sourceApps[i].Name).replace("{Class}", Itemclass);
        }

        var itemsStatus = '';
        for (var i = 0; i < statuApps.length; i++) {
            var Itemclass = "js-statusitem";
            if (statuApps[i].ID == "-1") {
                Itemclass += " active js-statusall";
            }

            itemsStatus += itemHtml.replace("{ID}", statuApps[i].ID).replace("{Name}", statuApps[i].Name).replace("{Class}", Itemclass);
        }

        baseHtml = baseHtml.replace("{FilterItems}", itemSources).replace("{StatusItems}", itemsStatus);
        return baseHtml;
    }

    function buildDefaultActionBarHtml() {
        var baseHtml = '';

        baseHtml = baseHtml + '<div id="actionBar" class="action-bar-container">';

        baseHtml = baseHtml + '<div class="action-bar-toggle"></div>';

        baseHtml = baseHtml + '<ul id="js-actionBarContainer" class="action-bar">';
        baseHtml = baseHtml + '</ul>';

        baseHtml = baseHtml + '    <div id="js-popover-content" class="hide">';
        baseHtml = baseHtml + '    </div>';

        baseHtml = baseHtml + '</div>';


        return baseHtml;
    }

    function buildDefaultHtmlFeedItem() {

        var itemActionHtml = '<div class="item-alert-popover">';
        itemActionHtml = itemActionHtml + '<div id="{notificationId}" status="{statusid}" linkto="{linkto}" itemgroup="{group}" class="item js-feed-action-item js-{groupClass}">';
        itemActionHtml = itemActionHtml + '<span class="{icon-importantClass}"></span>';

        itemActionHtml = itemActionHtml + '<div class="item-header">';
        itemActionHtml = itemActionHtml + '<span class="item-title">{itemIcon}</span>';
        itemActionHtml = itemActionHtml + '<div><span class="show-time">{notificationDTS}</span>';
        itemActionHtml = itemActionHtml + '<div class="show-action"><a id="{notificationId-mark}-mark" href="#" class="action-link {mark-link} js-feed-action-item-mark">{markText}</a>';
        itemActionHtml = itemActionHtml + '<a href="#" class="action-link action-link-danger js-feed-action-item-delete">{clearText}</a></div></div>';
        itemActionHtml = itemActionHtml + '</div>';

        itemActionHtml = itemActionHtml + '<div class="item-content">';
        itemActionHtml = itemActionHtml + '<span id="{notificationId-bubble}-bubble" class="{unread-class}"></span><span class="name-category">{categoryName}</span></br>';
        itemActionHtml = itemActionHtml + '<span class="des">{notificationText}</span>';
        itemActionHtml = itemActionHtml + '<div class="show-action action-view"><a id="notificationId-view" href="#"  class="action-link {action-link}">View</a></div>';
        itemActionHtml = itemActionHtml + '</div>';
        itemActionHtml = itemActionHtml + '</div>';

        itemActionHtml = itemActionHtml + '</div>';

        return itemActionHtml;
    }

    function buildAlertButton(actionItem) {
        var icon = "icon-alert";

        var itemActionHtml = '<div id="' + actionItem.id + '" class="alert-feed">';
        //itemActionHtml = itemActionHtml + '    <div class="dropdown-group-content ">';
        //itemActionHtml = itemActionHtml + '       <ul><li id="lkOpenFeed">' + ActionContent.OpenFeed + '</li><li id="lkDismissAll">' + ActionContent.DismissAll + '</li><li id="lkCloseFeed">' + ActionContent.CloseFeed + '</li></ul> ';
        //itemActionHtml = itemActionHtml + '    </div>';

        itemActionHtml = itemActionHtml + '<span data-container="body" data-toggle="popover" data-placement="top">';
        itemActionHtml = itemActionHtml + '     <span class="' + icon + '"></span>';
        itemActionHtml = itemActionHtml + '         <span id="' + actionItem.id + 'bubble" class="new"></span>';
        itemActionHtml = itemActionHtml + '     </span>';
        itemActionHtml = itemActionHtml + '</span>';
        itemActionHtml = itemActionHtml + '    <div id="js-popover-content" class="hide">';

        itemActionHtml = itemActionHtml + '    </div>';
        itemActionHtml = itemActionHtml + '</div>';

        return itemActionHtml;
    }

    function buildActionBarItem(actionItem) {
        var icon = actionItem.icon ? actionItem.icon : "";
        var tooltip = actionItem.tooltip ? actionItem.tooltip : "";
        var name = actionItem.name ? actionItem.name : "";

        var itemActionHtml = '<li id="' + actionItem.id + '" class="action-bar-item">';
        itemActionHtml = itemActionHtml + '<span class="btn" data-container="body" data-toggle="popover" data-placement="top">';
        itemActionHtml = itemActionHtml + '     <span class="' + icon + '"></span>' + name; // item action
        itemActionHtml = itemActionHtml + '         <span id="' + actionItem.id + 'bubble" class="bubble"></span>'; // icon bubble
        itemActionHtml = itemActionHtml + '     </span>';
        if (actionItem.tooltip != "") {
            itemActionHtml = itemActionHtml + '     <span class="tooltip-actionbar">' + tooltip + '</span> '; // tooltip 
        }
        itemActionHtml = itemActionHtml + '</span>';
        itemActionHtml = itemActionHtml + '</li>';

        return itemActionHtml;
    }

    function BuildAlertGroup(datas, feedHtmlTemplate) {
        var groupHtml = '<div groupname="{GroupName}" fdate="{FDate}" tdate="{TDate}" class="js-item-group-{Groupclass} title-group-alert"><span class="js-group">{Text}</span></div>';
        var stringHtml = "";
        var toDay = '';
        var yesterday = '';
        var thisWeek = '';
        var lastWeek = '';
        var twoWeeksAgo = '';
        var threeWeeksAgo = '';
        var thisMonth = '';
        var lastMonth = '';
        var older = '';
        var currDay = new Date();
        for (var i = 0; i < datas.length; i++) {
            var dataItem = datas[i];
            var itemDate = getDateTime(dataItem.CreatedDTS);
            var moduleCode = dataItem.SourceBriefName ? dataItem.SourceName : "";
            var importantClass = dataItem.IsImportant ? "icon-important" : "";
            var isUnread = dataItem.StatusID == Status.PENDING || dataItem.StatusID == Status.VIEW;
            var unreadClass = isUnread ? "pending" : "";
            var linkclass = "js-feed-action-item-view";
            if (dataItem.AlertType && dataItem.AlertType == AlertType.INFORMATION) {
                linkclass = "disabled";
            }
            var markLinkClass = isUnread ? "" : "hide";
            var itemActionHtml = feedHtmlTemplate
                .replace("{notificationText}", enCodeHtml(dataItem.AlertBody))
                .replace("{notificationId}", dataItem.NotificationAlertMsgID)
                .replace("{notificationDTS}", convertTimeSpanToDate(dataItem.CreatedDTS))
                .replace("{itemIcon}", moduleCode)
                .replace("{statusid}", dataItem.StatusID)
                .replace("{linkto}", dataItem.AlertMsgLink)
                .replace("{action-link}", linkclass)
                .replace("{clearText}", ActionContent.Clear)
                .replace("{categoryName}", dataItem.CategoryName)
                .replace("{markText}", ActionContent.MarkRead)
                .replace("{mark-link}", markLinkClass)
                .replace("{icon-importantClass}", importantClass)
                .replace("{unread-class}", unreadClass)
                .replace("{notificationId-mark}", dataItem.NotificationAlertMsgID)
                .replace("{notificationId-bubble}", dataItem.NotificationAlertMsgID);




            if (currDay.getDate() == itemDate.getDate()) {
                toDay += itemActionHtml.replace('{group}', "toDay").replace('{groupClass}', "toDay");
            } else {
                if (checkYesterDay(itemDate)) {
                    yesterday += itemActionHtml.replace("{group}", "yesterday").replace('{groupClass}', "yesterday");
                } else {
                    if (checkThisWeek(itemDate)) {
                        thisWeek += itemActionHtml.replace("{group}", "thisWeek").replace('{groupClass}', "thisWeek");
                    } else {
                        if (checkLastWeek(itemDate, 1)) {
                            lastWeek += itemActionHtml.replace("{group}", "lastWeek").replace('{groupClass}', "lastWeek");
                        } else {
                            if (checkLastWeek(itemDate, 2)) {
                                twoWeeksAgo += itemActionHtml.replace("{group}", "twoWeeksAgo").replace('{groupClass}', "twoWeeksAgo");
                            } else {
                                if (checkLastWeek(itemDate, 3)) {
                                    threeWeeksAgo += itemActionHtml.replace("{group}", "threeWeeksAgo").replace('{groupClass}', "threeWeeksAgo");
                                } else {
                                    if (checkThisMonth(itemDate)) {
                                        thisMonth += itemActionHtml.replace("{group}", "thisMonth").replace('{groupClass}', "thisMonth");
                                    } else {
                                        if (checkLastMonth(itemDate)) {
                                            lastMonth += itemActionHtml.replace("{group}", "lastMonth").replace('{groupClass}', "lastMonth");
                                        } else {
                                            older += itemActionHtml.replace("{group}", "older").replace('{groupClass}', "older");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }


        }

        if (SortValue == SortDes.DESC) {
            if (toDay != "") stringHtml += groupHtml.replace("{Text}", ActionContent.ToDay).replace("{Groupclass}", "toDay").replace("{GroupName}", "toDay").replace("{FDate}", GetGroupFDate('toDay')).replace("{TDate}", GetGroupTDate('toDay')) + toDay;
            if (yesterday != "") stringHtml += groupHtml.replace("{Text}", ActionContent.Yesterday).replace("{Groupclass}", "yesterday").replace("{GroupName}", "yesterday").replace("{FDate}", GetGroupFDate('yesterday')).replace("{TDate}", GetGroupTDate('yesterday')) + yesterday;
            if (thisWeek != "") stringHtml += groupHtml.replace("{Text}", ActionContent.ThisWeek).replace("{Groupclass}", "thisWeek").replace("{GroupName}", "thisWeek").replace("{FDate}", GetGroupFDate('thisWeek')).replace("{TDate}", GetGroupTDate('thisWeek')) + thisWeek;
            if (lastWeek != "") stringHtml += groupHtml.replace("{Text}", ActionContent.LastWeek).replace("{Groupclass}", "lastWeek").replace("{GroupName}", "lastWeek").replace("{FDate}", GetGroupFDate('lastWeek')).replace("{TDate}", GetGroupTDate('lastWeek')) + lastWeek;
            if (twoWeeksAgo != "") stringHtml += groupHtml.replace("{Text}", ActionContent.TwoWeeksAgo).replace("{Groupclass}", "twoWeeksAgo").replace("{GroupName}", "twoWeeksAgo").replace("{FDate}", GetGroupFDate('twoWeeksAgo')).replace("{TDate}", GetGroupTDate('twoWeeksAgo')) + twoWeeksAgo;
            if (threeWeeksAgo != "") stringHtml += groupHtml.replace("{Text}", ActionContent.ThreeWeeksAgo).replace("{Groupclass}", "threeWeeksAgo").replace("{GroupName}", "threeWeeksAgo").replace("{FDate}", GetGroupFDate('threeWeeksAgo')).replace("{TDate}", GetGroupTDate('threeWeeksAgo')) + threeWeeksAgo;
            if (thisMonth != "") stringHtml += groupHtml.replace("{Text}", ActionContent.ThisMonth).replace("{Groupclass}", "thisMonth").replace("{GroupName}", "thisMonth").replace("{FDate}", GetGroupFDate('thisMonth')).replace("{TDate}", GetGroupTDate('thisMonth')) + thisMonth;
            if (lastMonth != "") stringHtml += groupHtml.replace("{Text}", ActionContent.LastMonth).replace("{Groupclass}", "lastMonth").replace("{GroupName}", "lastMonth").replace("{FDate}", GetGroupFDate('lastMonth')).replace("{TDate}", GetGroupTDate('lastMonth')) + lastMonth;
            if (older != "") stringHtml += groupHtml.replace("{Text}", ActionContent.Older).replace("{Groupclass}", "older").replace("{GroupName}", "older").replace("{FDate}", GetGroupFDate('older')).replace("{TDate}", GetGroupTDate('older')) + older;
        } else {
            if (older != "") stringHtml += groupHtml.replace("{Text}", ActionContent.Older).replace("{Groupclass}", "older").replace("{GroupName}", "older").replace("{FDate}", GetGroupFDate('older')).replace("{TDate}", GetGroupTDate('older')) + older;
            if (lastMonth != "") stringHtml += groupHtml.replace("{Text}", ActionContent.LastMonth).replace("{Groupclass}", "lastMonth").replace("{GroupName}", "lastMonth").replace("{FDate}", GetGroupFDate('lastMonth')).replace("{TDate}", GetGroupTDate('lastMonth')) + lastMonth;
            if (thisMonth != "") stringHtml += groupHtml.replace("{Text}", ActionContent.ThisMonth).replace("{Groupclass}", "thisMonth").replace("{GroupName}", "thisMonth").replace("{FDate}", GetGroupFDate('thisMonth')).replace("{TDate}", GetGroupTDate('thisMonth')) + thisMonth;
            if (threeWeeksAgo != "") stringHtml += groupHtml.replace("{Text}", ActionContent.ThreeWeeksAgo).replace("{Groupclass}", "threeWeeksAgo").replace("{GroupName}", "threeWeeksAgo").replace("{FDate}", GetGroupFDate('threeWeeksAgo')).replace("{TDate}", GetGroupTDate('threeWeeksAgo')) + threeWeeksAgo;
            if (twoWeeksAgo != "") stringHtml += groupHtml.replace("{Text}", ActionContent.TwoWeeksAgo).replace("{Groupclass}", "twoWeeksAgo").replace("{GroupName}", "twoWeeksAgo").replace("{FDate}", GetGroupFDate('twoWeeksAgo')).replace("{TDate}", GetGroupTDate('twoWeeksAgo')) + twoWeeksAgo;
            if (lastWeek != "") stringHtml += groupHtml.replace("{Text}", ActionContent.LastWeek).replace("{Groupclass}", "lastWeek").replace("{GroupName}", "lastWeek").replace("{FDate}", GetGroupFDate('lastWeek')).replace("{TDate}", GetGroupTDate('lastWeek')) + lastWeek;
            if (thisWeek != "") stringHtml += groupHtml.replace("{Text}", ActionContent.ThisWeek).replace("{Groupclass}", "thisWeek").replace("{GroupName}", "thisWeek").replace("{FDate}", GetGroupFDate('thisWeek')).replace("{TDate}", GetGroupTDate('thisWeek')) + thisWeek;
            if (yesterday != "") stringHtml += groupHtml.replace("{Text}", ActionContent.Yesterday).replace("{Groupclass}", "yesterday").replace("{GroupName}", "yesterday").replace("{FDate}", GetGroupFDate('yesterday')).replace("{TDate}", GetGroupTDate('yesterday')) + yesterday;
            if (toDay != "") stringHtml += groupHtml.replace("{Text}", ActionContent.ToDay).replace("{Groupclass}", "toDay").replace("{GroupName}", "toDay").replace("{FDate}", GetGroupFDate('toDay')).replace("{TDate}", GetGroupTDate('toDay')) + toDay;
        }

        return stringHtml;
    }

    function BuildRunTimeAlertItem(alertdata, feedHtmlTemplate) {
        if (!feedHtmlTemplate) feedHtmlTemplate = buildDefaultHtmlFeedItem();
        var moduleCode = alertdata.SourceBriefName ? alertdata.SourceName : "";
        var groupName = GetGroupName(getDateTime(alertdata.CreatedDTS));
        var linkclass = "js-feed-action-item-view";
        if (alertdata.AlertType && alertdata.AlertType == AlertType.INFORMATION) {
            linkclass = "disabled";
        }

        var isUnread = alertdata.StatusID == Status.PENDING || alertdata.StatusID == Status.VIEW;
        var markLinkClass = isUnread ? "" : "hide";
        var unreadClass = isUnread ? "pending" : "";
        var importantClass = alertdata.IsImportant ? "icon-important" : "";
        var itemActionHtml = feedHtmlTemplate
               .replace("{notificationText}", enCodeHtml(alertdata.AlertBody))
               .replace("{notificationId}", alertdata.NotificationAlertMsgID)
               .replace("{notificationDTS}", convertTimeSpanToDate(alertdata.CreatedDTS))
               .replace("{itemIcon}", moduleCode)
               .replace("{statusid}", alertdata.StatusID)
               .replace("{linkto}", alertdata.AlertMsgLink)
               .replace("{action-link}", linkclass)
               .replace('{group}', groupName).replace('{groupClass}', groupName)
               .replace("{clearText}", ActionContent.Clear)
               .replace("{categoryName}", alertdata.CategoryName)
               .replace("{markText}", ActionContent.MarkRead)
               .replace("{mark-link}", markLinkClass)
               .replace("{icon-importantClass}", importantClass)
               .replace("{unread-class}", unreadClass)
               .replace("{notificationId-mark}", alertdata.NotificationAlertMsgID)
               .replace("{notificationId-bubble}", alertdata.NotificationAlertMsgID);

        return itemActionHtml;
    }

    //Feed Common Functions

    function GetGroupName(itemDate) {
        var currDay = new Date();
        if (currDay.getDate() == itemDate.getDate()) {
            return "toDay";
        } else {
            if (checkYesterDay(itemDate)) {
                return "yesterday";
            } else {
                if (checkThisWeek(itemDate)) {
                    return "thisWeek";
                } else {
                    if (checkLastWeek(itemDate, 1)) {
                        return "lastWeek";
                    } else {
                        if (checkLastWeek(itemDate, 2)) {
                            return "twoWeeksAgo";
                        } else {
                            if (checkLastWeek(itemDate, 3)) {
                                return "threeWeeksAgo";
                            } else {
                                if (checkThisMonth(itemDate)) {
                                    return "thisMonth";
                                } else {
                                    if (checkLastMonth(itemDate)) {
                                        return "lastMonth";
                                    } else {
                                        return "older";
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    function GetGroupContent(groupname) {
        if (groupname == "toDay") return ActionContent.ToDay;
        if (groupname == "yesterday") return ActionContent.Yesterday;
        if (groupname == "thisWeek") return ActionContent.ThisWeek;
        if (groupname == "lastWeek") return ActionContent.LastWeek;
        if (groupname == "twoWeeksAgo") return ActionContent.TwoWeeksAgo;
        if (groupname == "threeWeeksAgo") return ActionContent.ThreeWeeksAgo;
        if (groupname == "thisMonth") return ActionContent.ThisMonth;
        if (groupname == "lastMonth") return ActionContent.LastMonth;
        return ActionContent.Older;
    }

    function GetGroupFDate(groupname) {
        var currDay = new Date();
        var firstWeekday = getFirstWeekDay(currDay);
        var firstMonthday = new Date(currDay.getFullYear(), currDay.getMonth(), 1);
        var firstlastMonthday = getFirstlastMonthday(currDay);
        switch (groupname) {
            case "toDay":
                return formatServerDate(currDay);
            case "yesterday":
                return formatServerDate(addDays(currDay, -1));
            case "thisWeek":
                return formatServerDate(firstWeekday);
            case "lastWeek":
                return formatServerDate(addDays(firstWeekday, -7));
            case "twoWeeksAgo":
                return formatServerDate(addDays(firstWeekday, -14));
            case "threeWeeksAgo":
                return formatServerDate(addDays(firstWeekday, -21));
            case "thisMonth":
                return formatServerDate(firstMonthday);
            case "lastMonth":
                return formatServerDate(firstlastMonthday);
            default:
                return formatServerDate(new Date(1900, 1, 1));
        }
    }

    function GetGroupTDate(groupname) {
        var currDay = new Date();
        var firstWeekday = getFirstWeekDay(currDay);
        var firstMonthday = new Date(currDay.getFullYear(), currDay.getMonth(), 1);
        var firstlastMonthday = getFirstlastMonthday(currDay);
        switch (groupname) {
            case "toDay":
                return formatServerDate(currDay);
            case "yesterday":
                return formatServerDate(addDays(currDay, -1));
            case "thisWeek":
                return formatServerDate(addDays(currDay, -2));
            case "lastWeek":
                return formatServerDate(addDays(firstWeekday, -1));
            case "twoWeeksAgo":
                return formatServerDate(addDays(firstWeekday, -8));
            case "threeWeeksAgo":
                return formatServerDate(addDays(firstWeekday, -15));
            case "thisMonth":
                return formatServerDate(addDays(firstWeekday, -22));
            case "lastMonth":
                return formatServerDate(addDays(firstMonthday, -1));
            default:
                return formatServerDate(addDays(firstlastMonthday, -1));
        }
    }

    function formatServerDate(itemDate) {
        return itemDate.getMonth() + 1 + "-" + itemDate.getDate() + "-" + itemDate.getFullYear();
    }

    function addDays(currDate, days) {
        return new Date(currDate.getTime() + days * 24 * 60 * 60 * 1000);
    }

    function getWeekNo(date) {
        var firstWeekday = new Date(date.getFullYear(), date.getMonth(), 1).getDay();
        var offsetDate = date.getDate() + firstWeekday - 1;
        return Math.floor(offsetDate / 7);
    }

    function getFirstWeekDay(date) {
        var curDay = date.getDay();
        return addDays(date, 0 - curDay);
    }

    function getFirstlastMonthday(itemdate) {
        var lastmonth = itemdate.getMonth() - 1;
        var thisYear = itemdate.getFullYear();
        if (itemdate.getMonth() == 1) {
            lastmonth = 12;
            thisYear = thisYear - 1;
        }

        return new Date(thisYear, lastmonth, 1);
    }

    function checkYesterDay(itemDate) {
        var currDay = new Date();
        if (currDay.getYear() != itemDate.getYear()) return false;
        if (currDay.getMonth() != itemDate.getMonth()) return false;
        if (currDay.getDate() > itemDate.getDate() && currDay.getDate() == addDays(itemDate, 1).getDate()) {
            if (itemDate.getDay() < currDay.getDay()) {
                return true;
            }
        }
        return false;
    }

    function checkThisWeek(itemDate) {
        var currDay = new Date();
        if (currDay.getYear() != itemDate.getYear()) return false;
        if (currDay.getMonth() != itemDate.getMonth()) return false;
        if (getWeekNo(currDay) == getWeekNo(itemDate)) {
            if (addDays(itemDate, 2).getDate() <= currDay.getDate()) {
                return true;
            }
        }
        return false;
    }

    function checkLastWeek(itemDate, week) {
        var currDay = new Date();
        if (currDay.getYear() != itemDate.getYear()) return false;
        if (currDay.getMonth() == itemDate.getMonth()) {
            if (getWeekNo(currDay) - week < 0) return false;
            if (getWeekNo(currDay) == getWeekNo(itemDate) + week) {
                return true;
            }
        }
        return false;
    }

    function checkThisMonth(itemDate) {
        var currDay = new Date();
        if (currDay.getYear() != itemDate.getYear()) return false;
        if (currDay.getMonth() == itemDate.getMonth()) {
            return true;
        }
        return false;
    }

    function checkLastMonth(itemDate) {
        var currDay = new Date();
        if (currDay.getMonth() > 1 && currDay.getYear() == itemDate.getYear()) {
            if (currDay.getMonth() - 1 == itemDate.getMonth()) return true;
        } else {
            if (itemDate.getMonth() == 12 && currDay.getYear() == itemDate.getYear() + 1) return true;
        }

        return false;
    }
}

var actionBarModule = new ActionBarModule();
var alertDatas;
var AlertModule = function (options) {
    var self = this;
    var alertOption = $.extend({
        getUri: "",
        updateUri: "",
        alertTime: "5000", //auto hide affter this time
        fadeTime: "100",
        fadeOnHover: false,
        position: 'top-right',
        closeButton: true,
        closeOnClick: false,
        zIndex: '999',
        CloseText: "",
        ViewText: "",
        DismissText: "",
        PendingStatus: 5,
    }, options);

    self.buildAlertMsg = function () {
        buildAlertMsg();
    }
    self.reBuildAlertMsg = function () {
        buildAlertMsg();
    }
    self.dismissAll = function () {
        updateAlertRead("");
    }
    self.canRemoveAlertMsg = function (alertIds, isCheckPersistant) {
        //canRemoveAlert(alertIds, isCheckPersistant);
        reBinddAlert(alertIds, isCheckPersistant);
    }

    function buildAlertItemList(datas) {
        var groups = Object.create(null);
        for (var i = 0; i < datas.length; i++) {
            var dataItem = datas[i];
            if (!groups[dataItem.IsImportant]) {
                groups[dataItem.IsImportant] = [];
            }
            var categoryCode = dataItem.SourceId + "-" + dataItem.CategoryID + "-" + dataItem.IsAlertCanDismiss.toString();
            groups[dataItem.IsImportant].push({
                AlertBody: enCodeHtml(dataItem.AlertBody),
                IsAlertPersistence: dataItem.IsAlertPersistence,
                IsAlertCanDismiss: dataItem.IsAlertCanDismiss,
                IsImportant: dataItem.IsImportant,
                AlertMsgLink: dataItem.AlertMsgLink,
                NotificationAlertMsgID: dataItem.NotificationAlertMsgID,
                AlertLink: dataItem.AlertLink,
                AlertId: dataItem.AlertId,
                SourceName: dataItem.SourceName,
                CategoryName: dataItem.CategoryName,
                StatusID: dataItem.StatusID,
                SourceBriefName: dataItem.SourceBriefName,
                CreatedDTS: dataItem.CreatedDTS,
                AlertType: dataItem.AlertType,
                CategoryCode: categoryCode,
                TotalRows: datas.length
            });
        }
        var maxAlert = maxAlertCanShow() + 1;
        var alertCount = 0;

        //Important
        if (groups.true) {
            var objItemList = groups.true;
            alertCount = objItemList.length;
            if (alertCount > maxAlert) {
                alertCount = Math.floor(maxAlert) + 1;
            }
            var startbind = objItemList.length - alertCount > 0 ? objItemList.length - alertCount : 0;

            for (var l = startbind; l < objItemList.length ; l++) {
                var xitem = objItemList[l];
                var autoHide = xitem.IsAlertPersistence ? false : true;
                var alertType = xitem.AlertType ? xitem.AlertType : AlertType.ACTION;
                openAlertMsg(xitem.AlertBody, alertType, AlertMode.SINGLE, xitem.IsAlertCanDismiss, autoHide,
                    xitem.IsImportant, xitem.AlertMsgLink, xitem.NotificationAlertMsgID, xitem.SourceName,
                    xitem.CategoryName, true, AlertGroupType.None, xitem.StatusID, xitem.SourceBriefName, xitem.CreatedDTS, xitem.CategoryCode);
            }
        }

        if (groups.false) {
            //Not Important
            if (alertCount < maxAlert) {
                var objItemList = groups.false;
                //create group
                var groupCategory = [];
                for (var i = 0; i < objItemList.length; i++) {
                    var dataItem = objItemList[i];
                    var group = null;
                    var foundIdx = -1;
                    for (var j = 0; j < groupCategory.length; j++) {
                        if (groupCategory[j].code == dataItem.CategoryCode) {
                            group = groupCategory[j];
                            foundIdx = j;
                            break;
                        }
                    }

                    var alertItem = {
                        AlertBody: enCodeHtml(dataItem.AlertBody),
                        IsAlertPersistence: dataItem.IsAlertPersistence,
                        IsAlertCanDismiss: dataItem.IsAlertCanDismiss,
                        IsImportant: dataItem.IsImportant,
                        AlertMsgLink: dataItem.AlertMsgLink,
                        NotificationAlertMsgID: dataItem.NotificationAlertMsgID,
                        AlertLink: dataItem.AlertLink,
                        AlertId: dataItem.AlertId,
                        SourceName: dataItem.SourceName,
                        CategoryName: dataItem.CategoryName,
                        StatusID: dataItem.StatusID,
                        SourceBriefName: dataItem.SourceBriefName,
                        CreatedDTS: dataItem.CreatedDTS,
                        AlertType: dataItem.AlertType,
                        CategoryCode: dataItem.CategoryCode,
                        TotalRows: datas.length
                    };


                    if (foundIdx == -1) {
                        groupCategory.push({ code: dataItem.CategoryCode, value: [alertItem] });
                    } else {
                        groupCategory[foundIdx].value.push(alertItem);
                        groupCategory.push(groupCategory[foundIdx]);
                        groupCategory.splice(foundIdx, 1);
                    }
                }

                //Check start index
                var startbind = 0;
                var itemindex = objItemList.length;
                for (var j = groupCategory.length - 1; j >= 0; j--) {
                    var objGroupItem = groupCategory[j].value;
                    if (objGroupItem.length >= 5) {
                        alertCount += 1;
                    } else {
                        alertCount += objGroupItem.length;
                    }

                    itemindex = itemindex - objGroupItem.length;
                    if (alertCount > maxAlert) {
                        startbind = j;
                        break;
                    }
                }

                //Bind alert
                for (var x = itemindex; x < objItemList.length; x++) {
                    var xitem = objItemList[x];
                    var autoHide = xitem.IsAlertPersistence ? false : true;
                    var alertType = xitem.AlertType ? xitem.AlertType : AlertType.ACTION;
                    openAlertMsg(xitem.AlertBody, alertType, AlertMode.SINGLE, xitem.IsAlertCanDismiss, autoHide,
                        xitem.IsImportant, xitem.AlertMsgLink, xitem.NotificationAlertMsgID, xitem.SourceName,
                        xitem.CategoryName, true, AlertGroupType.None, xitem.StatusID, xitem.SourceBriefName, xitem.CreatedDTS, xitem.CategoryCode);
                }

            }
        }
    }

    var buildAlertMsg = function () {
        var urlServer = alertObj.getMsgUri;
        if (options) {
            urlServer = options.getUri;
        }
        var datas = {};
        var serverType = "GET";
        setTimeout(function () {
            alertAjax(urlServer, datas, function (data) {
                if (data) {
                    var objs = $('as-alert');
                    for (var k = 0; k < objs.length; k++) {
                        $(objs[k]).remove();
                    }
                    var datas = data.d ? data.d : data;
                    alertDatas = datas;
                    buildAlertItemList(datas);

                    reBuildAlert();
                }
            }, serverType);
        }, 0);

    }

    var openAlertMsg = function (alertMsg, alertType, alertMode, canDismiss, autoHide, isImportant, alertLink, alertId, sourceName,
        categoryName, display, groupType, statusId, moduleCode, createDts, categoryCode) {
        $.asAlert({
            updateRead: function (alertId, action) { updateAlertRead(alertId, action); },
            reBind: function (alertId) { reBinddAlert(alertId); },
            moduleName: sourceName,
            categoryName: categoryCode,
            categoryFullName: categoryName,
            moduleCode: moduleCode,
            createDts: createDts,
            msg: alertMsg,
            type: alertType,
            autoHide: autoHide,
            canDismiss: canDismiss,
            isImportant: isImportant,
            alertId: alertId,
            alertLink: alertLink,
            mode: alertMode,
            groupType: groupType,
            display: display,
            statusId: statusId,
            updateUri: alertOption.updateUri,
            alertTime: alertOption.alertTime,
            fadeTime: alertOption.fadeTime,
            fadeOnHover: alertOption.fadeOnHover,
            position: alertOption.position,
            closeButton: alertOption.closeButton,
            closeOnClick: alertOption.closeOnClick,
            zIndex: alertOption.zIndex,
            CloseText: alertOption.CloseText,
            ViewText: alertOption.ViewText,
            DismissText: alertOption.DismissText,
            PendingStatus: alertOption.PendingStatus,
        });
    }

    var reBuildAlert = function () {
        $.asAlert({
            BuildGroup: true
        });
    }

    var updateAlertRead = function (alertId, action) {
        var statusAction = action ? Status.READ : Status.VIEW;
        var datas = { alertId: alertId, status: statusAction, sourceIDs: "-1" };
        var serverType = "POST";
        var secparamobj = (typeof (updateSecAlertParams) != "undefined") ? updateSecAlertParams : '';
        var urlServer = alertObj.updateMsgStatusUri;
        if (options) {
            urlServer = options.updateUri;
        }

        alertAjax(urlServer, datas, function (data) {
            if (alertId == "") {
                buildAlertMsg();
            }
        }, serverType, secparamobj);
    }

    var reBinddAlert = function (ids, isPersistant) {
        //Remove 
        if (!alertDatas) return;
        var alertIdList = ids.split(',');
        if (isPersistant) {
            for (var i = 0; i < alertDatas.length; i++) {
                var item = alertDatas[i];
                if (!item.IsAlertPersistence) {
                    alertDatas.splice(i, 1);
                }
            }
        } else {
            for (var k = 0; k < alertIdList.length; k++) {
                var _id = alertIdList[k];
                for (var i = 0; i < alertDatas.length; i++) {
                    var item = alertDatas[i];
                    var alertid = item.NotificationAlertMsgID;
                    if (alertid == _id) {
                        alertDatas.splice(i, 1);
                    }
                }
            }
        }

        //ReBind
        var objs = $('as-alert');
        for (var j = 0; j < objs.length; j++) {
            $(objs[j]).remove();
        }
        buildAlertItemList(alertDatas);
        reBuildAlert();
    }

}
var alertModule = new AlertModule();

(function ($) {
    $.fn.asAlert = $.asAlert = function (arr) {
        var opt = $.extend({
            updateRead: function (alertId, action) { },
            reBind: function (alertId) { },
            moduleCode: "",
            createDts: "",
            moduleName: "",
            categoryName: "",
            categoryFullName: "",
            msg: "This is default as alert message.",
            type: "notification",
            mode: "single",
            groupType: "",
            display: true,
            autoHide: true,
            canDismiss: false,
            isImportant: false,
            alertId: "",
            statusId: "",
            deleteItem: "",
            checkPersistant: false,
            alertLink: "",
            alertTime: "10000",
            fadeTime: "300",
            closeButton: true,
            closeOnClick: false,
            fadeOnHover: true,
            position: 'top-right',
            zIndex: '999',
            CloseText: "Close",
            ViewText: "View",
            DismissText: "Dismiss",
            BuildGroup: false,
        }, arr);

        var alertPosition = (opt.position == 'bottom-right') ? 'bottom-right' : ((opt.position == 'bottom-left') ? 'bottom-left' : (opt.position == 'top-left') ? 'top-left' : 'top-right');
        if (opt.position == 'top-right' || opt.position == 'top-left') {
            reSetAlertPosition();
        }

        if (opt.deleteItem != "") {
            var alertList = $('as-alert');
            var alertIdList = opt.deleteItem.split(',');
            for (var j = 0; j < alertList.length; j++) {
                var alertid = $(alertList[j]).attr('alertid');
                var isDelete = false;
                for (var k = 0; k < alertIdList.length; k++) {
                    if (alertIdList[k] == alertid) {
                        isDelete = true;
                    }
                }

                if (isDelete) {
                    var categoryGroup = $(alertList[j]).attr('category');
                    var groupType = $(alertList[j]).attr('grouptype');
                    if (opt.checkPersistant) {
                        var autohide = $(alertList[j]).attr('autohide');
                        if (autohide == "false") continue;
                    }
                    $(alertList[j]).remove();
                    reCategoryGroup(categoryGroup, groupType);
                }
            }

            cleanRegularGroup();
            buildRegularGroup();
            return;
        }

        //if (opt.alertId) {
        //    var objs = $('as-alert');
        //    for (var i = 0; i < objs.length; i++) {
        //        var itemid = $(objs[i]).attr('alertid');
        //        if (opt.alertId == itemid) return null;
        //    }
        //}

        if (opt.BuildGroup) {

            return reBindAlert();
        }

        var fullClass = !opt.canDismiss ? "full-height" : "";
        var closeWithAction = '<span class=' + fullClass + '><a class="js-view">' + opt.ViewText + '</a></span>';
        if (opt.canDismiss) closeWithAction += '<span><a class="js-dismiss">' + opt.DismissText + '</a></span>';
        var closeHtml = opt.type == AlertType.INFORMATION ? '<span class="full-height close"><a class="js-close">' + opt.CloseText + '</a></span>' : closeWithAction;
        var timeStamp = $.now();
        var importantClass = opt.isImportant ? "important" : "not-important";
        var alertMaxLength = opt.isImportant ? 71 : 78;
        var alertDate = convertTimeSpanToDate(opt.createDts);
        var ext = {
            chkPosition: (opt.position == 'bottom-right') ? 'bottom-right' : ((opt.position == 'bottom-left') ? 'bottom-left' : (opt.position == 'top-left') ? 'top-left' : 'top-right'),
            closeOption: (opt.closeButton) ? '<as-alert-close>' + closeHtml + '</as-alert-close>' : '<style>#as' + timeStamp + ':before,#as' + timeStamp + ':after{display:none}</style>',
            chkMsg: (opt.msg.indexOf(" ") == 0) ? '' : '',
            iconType: opt.isImportant ? '<span class="important-red icon-important"></span>' : '<div></div>',
            alertHeader:
                '<div class="item-header">' +
                '<span class="item-title">' + opt.moduleName + '</span><span class="datetime-text">' + alertDate + '</span>' +
                '</div>',
            alertContent: '<div class="name-category">' + opt.categoryFullName + '</div><span class="des js-item-body" style="word-break:  break-word;" title="' + opt.msg + '">' + getAlertBody(opt.msg, alertMaxLength) + '</span>',

        };

        if ($('as-alert-box').length == 0) {
            $('body').append('<as-alert-box position="top-left" style="z-index:' + opt.zIndex + '"><as-alert-start></as-alert-start></as-alert-box><as-alert-box position="top-right" style="z-index:' + opt.zIndex + '"><as-alert-start></as-alert-start></as-alert-box><as-alert-box position="bottom-right" style="z-index:' + opt.zIndex + '"><as-alert-start></as-alert-start></as-alert-box><as-alert-box position="bottom-left" style="z-index:' + opt.zIndex + '"><as-alert-start></as-alert-start></as-alert-box>');
            reSetAlertBottom();
        }

        var asAlert = $('<as-alert id="as' + timeStamp + opt.alertId + '" display="' + opt.display + '" category="' + opt.categoryName + '" categoryName="' + opt.categoryFullName
            + '" grouptype="' + opt.groupType + '"  alertid="' + opt.alertId + '" alertdata="' + getAlertBody(opt.msg, alertMaxLength) + '"  important="' + opt.isImportant
            + '" autohide="' + opt.autoHide + '"  alertlink="' + opt.alertLink + '" class="item-alert-popover js-alert-' + opt.categoryName + ' ' + importantClass + '"  close-on-click='
            + opt.closeOnClick + ' fade-on-hover=' + opt.fadeOnHover + ' mode="' + opt.mode + '"type="' + opt.type + '" style="' + ext.chkMsg + '">'
            + ext.iconType + '<div class="noti-content item">' + ext.alertHeader + '<div class="item-content">'
            + ext.alertContent + ext.closeOption + '</div>' + '</div></as-alert>')
        .insertAfter('as-alert-box[position="' + ext.chkPosition + '"] > as-alert-start');

        if (opt.autoHide)
            setTimeout(function () {
                asAlert.fadeOut(opt.fadeTime, function () {
                    var tempObj = Object.create($(this));
                    var notificationId = $(this).attr('alertid');
                    if (opt.updateRead) {
                        opt.updateRead(notificationId);
                    }
                    $(this).remove();
                    reGoupAlert(tempObj);
                });
            }, opt.alertTime);

        $('as-alert[close-on-click="true"]').click(function () {
            $(this).fadeOut(opt.fadeTime, function () {
                $(this).remove();
            });
        });

        $('.js-view').unbind("click");
        $('.js-view').click(function () {
            $(this).parent().parent().parent().parent().parent()
            .fadeOut(0, function () {
                var tempObj = Object.create($(this));
                var notificationId = $(this).attr('alertid');

                if (opt.updateRead) {
                    opt.updateRead(notificationId, 1);
                }

                $(this).remove();
                var notificationLink = $(this).attr('alertlink');
                var groupType = $(this).attr('grouptype');
                var isDeleteGroup = groupType == AlertGroupType.Category ? true : false;
                reGoupAlert(tempObj, isDeleteGroup);
                if (notificationLink && groupType != AlertGroupType.Category) {
                    openAlertLink(notificationLink);

                    // Update feed
                    actionBarModule.updateFeedItem(notificationId);
                } else {
                    //Open feed
                    if ($('#' + alertObj.id).find('[data-toggle="popover"]')) {
                        $('#' + alertObj.id).find('[data-toggle="popover"]').popover("show");
                    }
                }

            });
        });

        $('.js-dismiss').unbind("click");
        $('.js-dismiss').click(function () {
            $(this).parent().parent().parent().parent().parent()
            .fadeOut(opt.fadeTime, function () {
                var tempObj = Object.create($(this));
                var notificationId = $(this).attr('alertid');
                if (opt.updateRead) {
                    opt.updateRead(notificationId);
                }
                $(this).remove();
                var groupType = $(this).attr('grouptype');
                var isDeleteGroup = groupType == AlertGroupType.Category ? true : false;
                reGoupAlert(tempObj, isDeleteGroup);
            });
        });

        $('.js-close').unbind("click");
        $('.js-close').click(function () {
            $(this).parent().parent().parent().parent().parent()
            .fadeOut(opt.fadeTime, function () {
                var notificationId = $(this).attr('alertid');
                var tempObj = Object.create($(this));
                if (opt.updateRead) {
                    opt.updateRead(notificationId);
                    reGoupAlert($(this));
                }

                $(this).remove();

                var groupType = $(this).attr('grouptype');
                var isDeleteGroup = groupType == AlertGroupType.Category ? true : false;
                reGoupAlert(tempObj, isDeleteGroup);
            });
        });

        function reGoupAlert(sender, isDelete) {
            var thisCategoryGroup = sender.attr('category');
            var groupType = sender.attr('grouptype');
            if (!thisCategoryGroup) return;

            var thisId = sender.attr('alertid');

            // Delete Category Group
            if (isDelete) {
                //deleteCategoryGroup(thisCategoryGroup, groupType);                
                if (groupType == AlertGroupType.Category) {
                    var groupItem = $('.js-alert-' + thisCategoryGroup + '.not-important');
                    for (var i = 0; i < groupItem.length; i++) {
                        if (thisId != "") thisId += ",";
                        thisId += $(groupItem[i]).attr('alertid');
                    }
                }
                opt.updateRead(thisId);
            }

            if (opt.reBind) {
                opt.reBind(thisId);
            }

            //cleanRegularGroup();
            //buildRegularGroup();
        }

        function reBindAlert() {
            cleanRegularGroup();
            builCategoryGroup();
            buildRegularGroup();
        }

        function builCategoryGroup() {
            var objs = $('as-alert[display="true"]');
            var objCategoryGroup = [];
            for (var i = 0; i < objs.length; i++) {
                var item = objs[i];
                var thisCategoryGroup = $(item).attr('category');
                var existCategoryGroup = false;
                for (var k = 0; k < objCategoryGroup.length; k++) {
                    if (objCategoryGroup[k].name == thisCategoryGroup) {
                        existCategoryGroup = true;
                    }
                }

                if (existCategoryGroup) {
                    continue;
                }

                if ($(item).attr('important') == 'true') {
                    continue;
                }

                var groupType = $(item).attr('grouptype');
                var mode = $(item).attr('mode');

                objCategoryGroup.push({
                    name: thisCategoryGroup
                });

                var objCategory = $('as-alert[category="' + thisCategoryGroup + '"]');
                objCategory = $('.js-alert-' + thisCategoryGroup + '.not-important');

                if (objCategory.length >= 5) {
                    for (var j = 0; j < objCategory.length; j++) {
                        var obj = objCategory[j];
                        $(obj).attr("grouptype", AlertGroupType.Category);
                        if (j == 0) {
                            // Create Group
                            var itemId = $(obj).attr('id');
                            var itemCategoryName = $(obj).attr('categoryName');
                            var groupBody = "You have " + objCategory.length + " " + itemCategoryName + " alerts";
                            $("#" + itemId + " .js-item-body").html(groupBody);
                            $(obj).attr("mode", AlertMode.GROUP);
                            $(obj).attr("display", true);
                        } else {
                            $(obj).attr("mode", AlertMode.SINGLE);
                            $(obj).attr("display", false);
                        }
                    }
                }
            }
        }

        function buildRegularGroup() {
            var canGroupNum = maxAlertCanShow();
            var objImportant = $('as-alert[important="true"]');
            for (var j = objImportant.length - 1; j >= 0 ; j--) {
                var it = objImportant[j];
                $(it).insertAfter('as-alert-box[position="' + alertPosition + '"] > as-alert-start');
            }

            // Build total
            var totalToastHtml = buildAlertTotal();
            if (totalToastHtml != '') {
                $(buildAlertTotal()).insertAfter('as-alert-box[position="' + alertPosition + '"] > as-alert-start');

                $("#divTotalToast").unbind("click");
                $("#divTotalToast").click(function (e) {
                    alertModule.dismissAll();
                });
            }

            var alertCangroup = 0;
            var objs = $('as-alert[display="true"]');
            for (var i = 0; i < objs.length; i++) {
                var item = objs[i];
                if (i + 1 >= canGroupNum) {
                    alertCangroup++;
                    if (alertCangroup == 1) {
                        if ($(item).attr("mode") == AlertMode.SINGLE && $(item).attr("grouptype") != AlertGroupType.Category) {
                            $(item).attr("display", true);
                            $(item).attr("mode", AlertMode.GROUP);
                            $(item).attr("grouptype", AlertGroupType.Regular);
                            $(item).addClass("js-" + AlertGroupType.Regular);
                        }

                    } else {
                        $(item).attr("display", false);
                        $(item).addClass("js-" + AlertGroupType.Regular);
                    }
                }
            }
        }

        function cleanRegularGroup() {

            var groupItem = $(".js-" + AlertGroupType.Regular);
            for (var i = 0; i < groupItem.length; i++) {
                var obj = groupItem[i];
                if ($(obj).attr('mode') == AlertMode.GROUP && $(obj).attr('grouptype') == AlertGroupType.Regular) {
                    $(obj).attr("mode", AlertMode.SINGLE);
                } else {
                    $(obj).attr("display", true);
                    $(obj).removeClass("js-" + AlertGroupType.Regular);
                }
            }
        }

        function reCategoryGroup(thisCategoryGroup, thisgroupType) {
            if (thisgroupType == AlertGroupType.Category) {
                var groupItem = $('.js-alert-' + thisCategoryGroup + '.not-important');
                if (groupItem.length >= 5) {
                    var objNew = groupItem[0];
                    // Create new group
                    var itemId = $(objNew).attr('id');
                    var groupBody = "You have " + groupItem.length + " " + thisCategoryGroup + " alerts";
                    $("#" + itemId + " .js-item-body").html(groupBody);
                    $(objNew).attr("mode", AlertMode.GROUP);
                    $(objNew).attr("display", true);
                } else {
                    for (var i = 0; i < groupItem.length; i++) {
                        var obj = groupItem[i];
                        if ($(obj).attr('mode') == AlertMode.GROUP) {
                            var gId = $(obj).attr('id');
                            $(obj).attr("mode", AlertMode.SINGLE);
                            $("#" + gId + " .js-item-body").html($(obj).attr('alertdata'));
                        }
                        $(obj).attr("display", true);
                        $(obj).attr("grouptype", AlertGroupType.None);
                    }
                }

            }
        }

        function deleteCategoryGroup(thisCategoryGroup, thisgroupType) {
            if (thisgroupType == AlertGroupType.Category) {
                var groupItem = $('.js-alert-' + thisCategoryGroup + '.not-important');
                for (var i = 0; i < groupItem.length; i++) {
                    var notificationId = $(groupItem[i]).attr('alertid');
                    if (opt.updateRead) {
                        opt.updateRead(notificationId);
                    }
                    $(groupItem[i]).remove();
                }
            }
        }

        function buildAlertTotal() {
            $("#divTotalToast").remove();
            var htmlString = "";
            var totalItem = alertDatas.length;
            if (totalItem > 0) {
                if (totalItem == 1) {
                    htmlString = "<div id='divTotalToast' class='total-alert btn btn-secondary btn-full mb-sm'>Dismiss Alert</div>";
                } else {
                    htmlString = "<div id='divTotalToast' class='total-alert btn btn-secondary btn-full mb-sm'>Dismiss All Alerts</div>";
                }
            } else {
                htmlString = '';
            }

            return htmlString;
        }

        return this;
    };

}(jQuery));

function showDismissAll(isActionOnFeed) {
    var objDislayTrue = $('as-alert[display="true"]');
    //var isOpenFeed = $("#div-alert").hasClass("js-feedopen");
    if (objDislayTrue.length) {
        $("#lkDismissAll").removeClass("hide");
    } else {
        $("#lkDismissAll").addClass("hide");
    }
}

function convertTimeSpanToDate(timeSpan) {
    var d = getDateTime(timeSpan);
    return formatDate(d, "dd MMM");
}

function getDateTime(timeSpan) {
    if (timeSpan) {
        timeSpan = Number(timeSpan.replace("/Date(", "").replace(")/", ""));
        return new Date(timeSpan);
    }
    return new Date();
}

function formatDate(date, stringFormat) {
    var shortMonths = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    var month = shortMonths[date.getMonth()];

    //New format: 07/17/2017
    month = date.getMonth() + 1;
    if (month < 10) month = "0" + month;
    var days = date.getDate();
    if (days < 10) days = "0" + days;
    var hours = date.getHours();
    var minutes = date.getMinutes();
    var ampm = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12;
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var strTime = hours + ':' + minutes + ' ' + ampm;
    //return days + " " + month + " " + strTime;
    return month + "/" + days + "/" + date.getFullYear().toString().substr(-2) + " " + strTime;
}

function reSetAlertPosition() {
    //var alertTop = 140;
    //var scrollTop = $(window).scrollTop();
    //if ($(".container")) {
    //    var offsetTop = $(".container").offset().top;
    //    alertTop = offsetTop - scrollTop;
    //    if (alertTop < 0) alertTop = 40;
    //}

    // CRs: 2017/07/26
    var alertTop = 35; // 10;
    var alertRigth = 10;
    $("as-alert-box[position='top-right']").css("top", alertTop + "px");
    $("as-alert-box[position='top-left']").css("top", alertTop + "px");

    $("as-alert-box[position='top-right']").css("right", alertRigth + "px");
    $("as-alert-box[position='top-left']").css("right", alertRigth + "px");
}

function maxAlertCanShow() {
    var pageHeader = 100;
    var menuNav = 0;
    if ($('#navBar').css('height')) {
        menuNav = Number($('#navBar').css('height').replace("px", ""));
    }
    var totalNav = 100;
    var docHeight = $(window).height() - pageHeader - menuNav - totalNav;
    var alertitem = $('as-alert');
    var itemHeight = 100;
    if (alertitem.length > 0) {
        itemHeight = Number($('as-alert').css('height').replace("px", "")) + 5;
    }
    return docHeight / itemHeight;
}

function expandData(href) {
    var elms = $("#myGroup").find(".collapse");
    $.each(elms, function () {
        var elm = $(this).attr('id');
        if (href === elm) {
            $("#" + elm).collapse("show");
        } else {
            $("#" + elm).collapse("hide");
        }
    });
}

function openAlertLink(notificationLink) {
    var openPopup = notificationLink.match(/OpenType=OpenPopup/g);
    var isFullLink = notificationLink.match(/http:\/\//g);
    var isFullLinks = notificationLink.match(/https:\/\//g);
    if (!isFullLink && !isFullLinks) {
        var domain = window.location.protocol + "//" + window.location.hostname + "/";
        if (subDomain && subDomain != "") {
            domain = domain + "/" + subDomain + "/";
        }
        notificationLink = domain + notificationLink;
    }

    if (openPopup) {
        openPopupWindowOnMenu(null, notificationLink);
    } else {
        setTimeout(function () {
            window.location = notificationLink;
        }, 1000);

    }
}

function getAlertBody(msg, maxLength) {
    if (msg) {
        var maxlength = maxLength ? maxLength : 100;
        msg = msg.replace('<br/>', '');
        if (msg.length > maxlength) {
            return msg.substring(0, maxlength - 3) + "...";
        }
    }

    return msg;
}

function alertAjax(url, datas, callback, type, securObj) {

    if (datas == undefined || datas == null) {
        datas = {};
    }

    var alertDatas = datas;
    if (securObj) {
        alertDatas = createSecData(datas, securObj);
        if (type == "POST") {
            //Add request token
            alertDatas = addTokenToParam(alertDatas);
        }
        $.ajax({
            type: type,
            url: url,
            data: alertDatas,
            cache: false,
            async: true,
            success: function (response) {
                if (callback) {
                    callback(response);
                }
            }
        });

    } else {
        if (type == "POST") {
            alertDatas = JSON.stringify(alertDatas);
        }

        $.ajax({
            type: type,
            url: url,
            data: alertDatas,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            cache: false,
            async: true,
            success: function (response) {
                if (callback) {
                    callback(response);
                }
            }
        });
    }
}

function createSecData(data, securObj) {
    var dataSec = {};
    switch (securObj.type) {
        case "getalertfeed":
            dataSec[securObj.pageNo] = data.pageNo;
            dataSec[securObj.pageSize] = data.pageSize;
            dataSec[securObj.isPaging] = data.isPaging;
            dataSec[securObj.maxIndex] = data.maxIndex;
            break;
        case "updatestatus":
            dataSec[securObj.alertId] = data.alertId;
            dataSec[securObj.status] = data.status;
        case "deletegroup":
            dataSec[securObj.fDate] = data.fDate;
            dataSec[securObj.tDate] = data.tDate;
        default:

    }

    return dataSec;
}

var reSetAlertBottom = function () {
    var actionbar = $('.action-bar-container');
    if (actionbar) {
        var status = actionbar.hasClass('open');
        if (status) {
            $("as-alert-box[position='bottom-right']").css("bottom", "7rem");
        } else {
            $("as-alert-box[position='bottom-right']").css("bottom", "4rem");
        }

        setCookie('actionBarOpen', status);
    }
}

function setCookie(name, value, days) {
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + value + expires + "; path=/";
}

function getCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

function enCodeHtml(stringHtml) {
    return $('<div/>').text(stringHtml).html();
}

function addTokenToParam(alertDatas) {
    alertDatas = boarding.addRequestVerificationToken(alertDatas);

    return alertDatas;
}