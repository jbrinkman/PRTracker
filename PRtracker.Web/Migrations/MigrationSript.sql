﻿DECLARE @CurrentMigration [nvarchar](max)

IF object_id('[dbo].[__MigrationHistory]') IS NOT NULL
    SELECT @CurrentMigration =
        (SELECT TOP (1) 
        [Project1].[MigrationId] AS [MigrationId]
        FROM ( SELECT 
        [Extent1].[MigrationId] AS [MigrationId]
        FROM [dbo].[__MigrationHistory] AS [Extent1]
        WHERE [Extent1].[ContextKey] = N'PRTracker.Web.Migrations.Configuration'
        )  AS [Project1]
        ORDER BY [Project1].[MigrationId] DESC)

IF @CurrentMigration IS NULL
    SET @CurrentMigration = '0'

IF @CurrentMigration < '201501060056027_InitialCreate'
BEGIN
    CREATE TABLE [dbo].[Notifications] (
        [Id] [int] NOT NULL IDENTITY,
        [RawJson] [nvarchar](max),
        [ReceivedDate] [datetime] NOT NULL,
        [RepositoryId] [int],
        CONSTRAINT [PK_dbo.Notifications] PRIMARY KEY ([Id])
    )
    CREATE INDEX [IX_RepositoryId] ON [dbo].[Notifications]([RepositoryId])
    CREATE TABLE [dbo].[Repositories] (
        [Id] [int] NOT NULL,
        [Name] [nvarchar](max),
        [Url] [nvarchar](max),
        CONSTRAINT [PK_dbo.Repositories] PRIMARY KEY ([Id])
    )
    CREATE TABLE [dbo].[PullRequests] (
        [Id] [int] NOT NULL,
        [RepositoryId] [int] NOT NULL,
        [CreatedByUserId] [int],
        [Number] [int] NOT NULL,
        [Closed] [bit] NOT NULL,
        [ClosedDate] [datetime],
        [Merged] [bit] NOT NULL,
        [MergedDate] [datetime],
        [MergedByUserId] [int],
        CONSTRAINT [PK_dbo.PullRequests] PRIMARY KEY ([Id])
    )
    CREATE INDEX [IX_RepositoryId] ON [dbo].[PullRequests]([RepositoryId])
    CREATE INDEX [IX_CreatedByUserId] ON [dbo].[PullRequests]([CreatedByUserId])
    CREATE INDEX [IX_MergedByUserId] ON [dbo].[PullRequests]([MergedByUserId])
    CREATE TABLE [dbo].[Users] (
        [Id] [int] NOT NULL,
        [Login] [nvarchar](max),
        [DnnLogin] [nvarchar](max),
        [Name] [nvarchar](max),
        CONSTRAINT [PK_dbo.Users] PRIMARY KEY ([Id])
    )
    CREATE TABLE [dbo].[RepositoryHistories] (
        [Id] [int] NOT NULL IDENTITY,
        [Forks] [int] NOT NULL,
        [Watchers] [int] NOT NULL,
        [CreatedDate] [datetime] NOT NULL,
        [RepositoryId] [int] NOT NULL,
        CONSTRAINT [PK_dbo.RepositoryHistories] PRIMARY KEY ([Id])
    )
    CREATE INDEX [IX_RepositoryId] ON [dbo].[RepositoryHistories]([RepositoryId])
    ALTER TABLE [dbo].[Notifications] ADD CONSTRAINT [FK_dbo.Notifications_dbo.Repositories_RepositoryId] FOREIGN KEY ([RepositoryId]) REFERENCES [dbo].[Repositories] ([Id])
    ALTER TABLE [dbo].[PullRequests] ADD CONSTRAINT [FK_dbo.PullRequests_dbo.Users_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [dbo].[Users] ([Id])
    ALTER TABLE [dbo].[PullRequests] ADD CONSTRAINT [FK_dbo.PullRequests_dbo.Users_MergedByUserId] FOREIGN KEY ([MergedByUserId]) REFERENCES [dbo].[Users] ([Id])
    ALTER TABLE [dbo].[PullRequests] ADD CONSTRAINT [FK_dbo.PullRequests_dbo.Repositories_RepositoryId] FOREIGN KEY ([RepositoryId]) REFERENCES [dbo].[Repositories] ([Id]) ON DELETE CASCADE
    ALTER TABLE [dbo].[RepositoryHistories] ADD CONSTRAINT [FK_dbo.RepositoryHistories_dbo.Repositories_RepositoryId] FOREIGN KEY ([RepositoryId]) REFERENCES [dbo].[Repositories] ([Id]) ON DELETE CASCADE
    CREATE TABLE [dbo].[__MigrationHistory] (
        [MigrationId] [nvarchar](150) NOT NULL,
        [ContextKey] [nvarchar](300) NOT NULL,
        [Model] [varbinary](max) NOT NULL,
        [ProductVersion] [nvarchar](32) NOT NULL,
        CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY ([MigrationId], [ContextKey])
    )
    INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
    VALUES (N'201501060056027_InitialCreate', N'PRTracker.Web.Migrations.Configuration',  0x1F8B0800000000000400ED5CDB6EDC36107D2FD07F10F4D416AED7765E5A633741B28E53B7B11D789DB66F0157A2D744246A2B51AE1745BFAC0FFDA4FE4247AB1B29921275D98B9320406053E470383C339CA18EFCDF3FFF8E5F3CFA9EF580C3880474621F1F1ED916A64EE012BA98D831BBFBFE07FBC5F3AFBF1ABF76FD47EBD7BCDFB3A41F8CA4D1C4BE676C793A1A45CE3DF65174E813270CA2E08E1D3A813F426E303A393AFA71747C3CC220C206599635BE8929233E5EFF02BF4E03EAE0258B917719B8D88BB27678325B4BB5AE908FA32572F0C47E43D84FF1FC5DEC7937F88F18472C3A3C430CD9D64B8F20506786BD3BDB4294060C3150F6F47D84672C0CE862B68406E4DDAE9618FADD212FC2D9224ECBEEA6EB393A49D6332A07E6A29C386281DF52E0F1B3CC40A3EAF04E66B60B0382095F83A9D92A59F5DA8C13FB2A60E48E3899F8EA84A7532F4C3A2B2D9D6ECF212FE1C092FB1D1448014025FF0EAC69ECB138C4138A631622EFC07A17CF3DE2FC8257B7C1474C271486F36A83E2F04C6880A67761B0C4215BDDE0BB6C3117AE6D8DC471A3EAC0621837265DE20565CF4E6CEB0A2647730F17A8E0CC31634188DF608A43C4B0FB0E3186439AC8C06BBB4AB357E6BA417FFE1C25764E2704248267D9D6257A7C8BE982DD4F6CF8D1B6CEC92376F3964C89F7948023C22016C6B8711EEC60F2805DF0059C4F96FC7C0B6EA6586093B065101158F84A325565E0157A208BB5A1B4226CEB067BEB2ED13D59A60E7A583EFEC08329024B84817F13788208B1CF875B142E3003C582868EB3200E9DCA6AC7A3D2216ADD845F41172729C77FA62E7215D046A425FF6FDC37DE87DEF07368915FC1F3D0E0CF31DD08FEDC4B4CF5E6D1A9549BEBF081778E5269750FC95F35DD54DE6A16627E22E09E21C14DE64EFBAD34DAD7F5AB89398ACEBDE20E679D6E81876BFB1279B4F3D49C712D4FCB698893D95FAD20D30C9B0ECC6AB088FD390E7BCEEF05112EA67D15003E11ED28459D3ED48FBCC4E01ABDE74FA5749FDFCCFCDA60226C6263F8ABF4564740A19314B7F53DDB066E7EFD8D9A8B9DD58AF37D6AF5163AB655BB2143EC7CDAD429AC08E89D62746ABD2EC13919F9252A6BE7791B2CC8E68BA5334AB733D106D2DB0EF54B96A1F42D6332319F297A4D0BFEF320FC18F53BD07F43CCB9077B0E92966CFA42A04952FF1B821E59BBBE58AA49F14DBCED6514050E59EB2C2DA752D489E6784D5DCBB4C24BC386680C881FE0586409AE046A4DECA3C3C363C9E6069314F54C3989782F284EF39D340778240E139740DE1404828F13CA64F725D4214BE419AEB932DE300024DB54CC547D7286979826BE6B681113154477909529E6ACC4A9269B8D471CAEEAE1A6CF46755030484D4B2C08456813144CE650E02D4DA20CE0DC096A8DAA6C016B8D2637D1412A2F778E37A186308182BAA018146DCA526427605369B265ACA9EC6DA242B592DE39D2B873CF0404AA937F5094A92E044D4EE9E3AAF9C7D7F40C7B9861EBA593BE259CA2C841AE9C3F4102E20E8C5079155BC6A7BC4F4FE4D4AD4D4A9B73B0BA0C5505A3A2703347ABD925F6D3C0ACC95AB69A31EAF76F2FF19B962E3086C1081CCAF13579841F55AF19E004C8EE06A2EC12A20AB844F40C33F54BAFB26852D616127A456185A1D66F7424593C641B2489EFB42449C289D0202A39135532D2C4C27445FCABAA9A85158E5F91CB6DB44A78F57522D7BDE9D563158D2DEAD462B1D59D9320DEA22EE5845616558D35A24D0CEC5573912F1BCCB0CE6A596971AB13215A63B2E6DA8A939AA175505B89AF0EEA4DA52F11DA15097D0DA52C0B366C273E3ED55B49973CB44970FB5A48911E183A740733D55FEAD585ABA67CAB7DC6A55A251F9E8DA2574D92D5DF8AF9E563718217CFC6A3941899358C471A06E5F8122D97842E384665D662CD523AE5F4FB597B8AA19FCA183991826958685BCC040B470B5C790A5383A6E7248C58C2DF9CA3E4EE78EAFA523745BEA2396CF309552989BC99F9E19B8F4A7E4E476A28A6621223677C99A07358AC9F648FEB17099A734C1E6E256457E4A150F1EE621A78B14FF5C9AC7E74413FE445148D2DE408F4424198F0A48D443E1B1625D6E5C909DA2B969632696973A5F244448C119E04271E024EFA506600A6BAC19B8152FA2E931F9FB6984B58F3F07801EB86BDD960E1F81C6283F9F2A2FD0ED78EDE50B4E8EC933A89D2E5352FB4F1665B2F37E74D0970CCDA5A689731A704A5B2B6B652E4C8C8B79B4BCBD954BCA4BCADAD145927BEBDAD34F52636DD19EFCC9DD3D47E083F561531060EAC1EB619CFCDE82CBC80ACC95C46C98AE1C594AD9B382A767D967309FFB0477A7E83D3E764D7CAD80C8632E60A2F206B32975152577831656BEB93431156F9074F3EDF946ABC6A9762F6A2D6ABD474E3ACBE6AFE744E2AB8D22EB605A67A206E526CCD5611C37E0AE6D91FDED423B0DEB2C325A2E40EE09E52B8EC93A3E393CA8777FBF311DC288A5C4F519FEABE8413F76C0B5C349258B6916DD696A9257E6F461F50E8DCA3F01B1F3D7ECB0BEBFA4D990B3FB3A128646B0B48EF1E2EA88B1F27F65FEB81A7D6C5EF1FF8B107D67508483CB58EACBFEB17D4E54BAFDD80A0A529795669AF0DE63E8C6A21A7D3972C4FC2B09BC6689FCF494CF5A80C37771705CE848F53BA1854FC34654EBA4A688A4026AB113F53E9A28BFC894A3F5DBA6EB1387A03015141ECDA538F1568FCBD6261F59B805EC23A46E81EDCFE4F237B11C8F35DF050A5CE770A5A32717E6FB31E85125BA1AC0FC34E175921BBA2A26F8343D4F0EA680FC8424350CC53664017D6ADF2DD7477F666274468AEF936C984FC9409E05FC0B009303C458A76EB636350EAE93EA069BB874D5B4CEDC159D39358BD5B5C29789BFDE8DC4F0063266F3AF600696A0AB4CC3CAA6EAD9EDDDC486E4E2FD5A16698078083344DAFA7B0EAD9C20DEC67D554B55CB31A7E74133D5A35571DE15049A0D6F2A755D2D5DCCC266A75BDCD8A3EB5A6D3D30077CCC0D6B0166BC98A2AF0CA7CB42740ABE60154BE6037E4B9EA88B2FBCF917E0ACB1E98F2DC0FF0DB59FED6A9CC5D4CA10D8E1AA6C146B8C9F2FB6938D0B93F000BB9454416A588E4CFC152EC084779D1E782DE05795E51D128EF22DDB533E4C239FF328470871C068F1D1C45EBBFC5F22BF262E8F2DA9F63F7825EC76C19335832F6E79E608C2433A99B7F4DC016751E5F2FD7717D8825809A24B982BCA6AF62E2B985DEE78ADB3F8D8824E5C92E6A93BD64C985ED62554892FF368F4E5066BE2253BBC5FED20361D1359DA107DC453708686FF10239AB9C66A017D2BC11A2D9C767042D42E447998C723CFC0A1876FDC7E7FF03157C545707590000 , N'6.1.2-31219')
END

IF @CurrentMigration < '201501102253130_ProcessedFlag'
BEGIN
    ALTER TABLE [dbo].[Notifications] ADD [Processed] [bit]
    INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
    VALUES (N'201501102253130_ProcessedFlag', N'PRTracker.Web.Migrations.Configuration',  0x1F8B0800000000000400ED5CCD6EDC3610BE17E83B083AB545B2B29D4B6BECB648D671EB36B603AFD3F61670257A4D44A2B612E57A51F4C97AE823F5153A5AFD91222951D2FE3909020436450E87C38FC319F2A3FFFBE7DFF10F8F816F3DE02826219DD8C7A323DBC2D40D3D4217133B6177CFBFB57FF8FECB2FC6AFBDE0D1FAB5A8F722AD072D693CB1EF195B9E3A4EECDEE300C5A380B8511887776CE4868183BCD039393AFACE393E763088B04196658D6F12CA4880D7BFC0AFD390BA78C912E45F861EF6E3BC1CBECCD652AD2B14E078895C3CB1DFDEDC46C8FD80A3D16F783E3A430CD9D64B9F20D06486FD3BDB4294860C31D0F3F45D8C672C0AE962B68402E4DFAE9618EADD213FC6B9FEA75575D3A11C9DA44371AA86852837895918741478FC22B78D536FDECBC276693BB0DE6BB0325BA5A35E5B70625F858CDC1137175FEFF074EA4769E5BA91B34919F18D9F5942956725340041E9BF67D634F15912E109C5098B900F0D92B94FDC5FF0EA36FC80E98426BECF2B0BEAC237A1008ADE46E112476C7583EFF2215C78B6E588ED9C7AC3B219D7261BD805652F4E6CEB0A3A47731F9758E08C306361847FC414478861EF2D620C47349581D7D6947AAFF57583FEFC394EAD9B7508F883A5645B97E8F10DA60B763FB1E147DB3A278FD82B4A7225DE51022B0F1AB128C1ADFD60179307ECC10AC04567E9CFB7B0AE14036C13B60C6302035F49A66A6E08BFBA388E71D9EA5518FA1851A9DD157A208BB581B55DDBD60DF6D755E27BB2CC96F3A8FAFC9EC75F0C168CC2E026F40511629DF7B7285A6006AA852D15676112B9352B8D9D6AF9342E2A7E041D9754D5F4D35B5057216DC565FAFFD657D2BBC8DF7C1F5ABCD750BC69C817486E857CB1364CF57E0B337C83FF4870CCD46A7315DEF34BA2525A5D435AA59A6AAA356AE6587E22B0282382DBCC9DD55B69B46FAAD7E069149507791BCE3A9DDD0DD7F6B3BF51F5D3B00F76DC51A7114E7B7FB5821834EAB8A95E25C11C4703FBF743D5AEDC4B8A3AC4686E798961410CEE3F93D2BF7F33F36B5D883089AD4EAF565BEDF7844A92B7D6D7ECEAAEF9F1B76A2E56562BCED769D45BA8D855ED9668B0F71ED3A4B0C28DF7F2CC99F53ABAE4B4D1675FACEAE74DB820DB4FA3CE28DD4D475B08657B64287934322051C9257C7A98353D00380FA30FF1B0CDFB37C4DC7BB0E74642906D1F10B4491A9EF90F88CBF5E95043106FB2C65EC671E892B5CED2706A699B688ED7D4B34C73B8CC5988C600AF010B8B2C6129815A13FB68343A966C6ED04999B1549D88A7836237DF487DC08AC451BA24903F0581B0C60965F2F225D4254BE41B8EB9D6DED001A4D354F654FF72869798A66BD7D022262A88CB4156A6ECB3E6A7DA6C3676385C35C34D1F79EAA0601086565810D2CC362898F4A1C05B163019C0B917D45A55D901D65A4D6EA283944AEE1D6F42BE60020575F2B051B429D38EBD804DA5C98EB1A6B2B7890AF5AC79EF48E3F63D1310A876FE8DA24C75E467B24B1FD7CD3FBEA667D8C70C5B2FDDECAE708A62177972FC040188B76184CAA3D8313EE5797A22BB6E6350DA1E833545A82A1895E99A395ACD8EA99F06664DC6B2D388513F7F0789DF2C7581360C5AE048F6AFE927FCA8BA48801D203F1188F3A3873AE052D133CCD4D75A55D2A4CC2D24F48AC24A43ADEF6C24593C645B2489B75692246147681195EE892A195960613A22FE32AA6160E5C2AFC9E5265A25BC7E61C8556FBB5CACA3B1439E5A0EB63E7312C43BE4A59CD0DAA0EABE46B48981BD1A0EED658319E6591D332D6E7422441B4CD69E5B715273B46ED456E23541B3A9F42942B72461A8A19469C196EDC4FBA7662BE982872E01EE500B29C203C305DDC34CCD877A4DEEAA2DDEEA1E71A946C9BB6723EFD510640DB76271F858EEE0E5B7B1933123F382B1A3A1508E2FD17249E882A354E625D62CE3534E9FCFBA130D834C86E3C60ABE61A96DD9130C1C2D70ED2B740D9A9E932866298B738ED2B3E3A91748D514F18A66B32D3A548524F264169B6FD12AFD395FCC12C7548C5FE4602F97710EE30CD2C0717D87A0D9C2E4E6564A74453E8A14D716D3D04F02AA8F63F5AD4B26222FA22CEC2047601A0AC2842F5D24F281B028B12944D64BE47888BC38AE589635766AB32605E41246A42C47049E112C055F3010957A676880C9A6C6DB41647607CAB7CF4ACC25ACB97ABC8075C1C1CCADB0010F9C5B3E37E93EB98DADB7E46F36BEAAA5936F5E68EBB1B85E6E41B01290989775D02EA758094AE5655DA5C8BE952F379756D0AE784945595729B24E7C795769EA496C3B70DEDB4ACEF282814B5895FC18AC5D75B3ED2CDA9CFCC20BC88BCC65541C1A5E4C55BA8D0D62DF9B3797286C6C0F2F0E7D866CE55A19DB814F4E76E105E445E6322AB60B2FA62AEDBC5F289C29FF61173BDA56E129A585F52A65EF657A584B03C7794AD6FEDC4ECAD1B22AB605A67A205E9A9FCD5631C34106E6D91FFED42730DEAAC225A2E40EC2908CF5659F1C1D9FD45EEC1DCEEB39278E3D5F91D2EA9ED08973B603FA1A492DDB4A50EB4AEE129FACD10714B9F728FA2A408F5FF3C2FA3E4BF3E067B629D6D9DA02D275C505F5F0E3C4FE6BDDF0D4BAF8FD3DDFF699751D01124FAD23EBEFAE03925EB8CD096B96D1E7E1D87E80D4713A780AEB2090702FAE3AC8E9F544E6491876DB381FF262C5548F5AF3214B4E7CFFD2C7A0E2EB97DA8AEDF9F245E5C54C4623BE84E9A38BFC0A66982E7DA7586C6D3EC3DD5E503C89152BBC1918E40BEB0F100609EBE9A1073C24F838222081B3DF070F75C67E2FA725F3F50F36725228B113A6FC6648F12219655F0CF85D50975AAEAD0E80A3B409667B4648E843F6555E89F7278DF64284E694709B04CC8F9977FE190CDB00C353648677DE3636CA783D0434ED76B3E98AA903D86B06F2B9F78B2B055D74188BFC0960CCE4B6E40090A6665ECB84A7FAD4EA49D5AD9CEAEC601E7286790838C8C2F466E6AC9EA4DC42BA5675D548716BA065B7B1B2557D35F11C95BC6D2D6D5B255D4D096D637437DBACACD3683A3DFB70CFC46F0D59B29123A902AFCC857B026C6E1E40D5FDBC21BD56C7CF3D7C6AF65318F68699D6C300BF9BE1EF9C41DDC7145AE7A8612B6C85122DDF71C386CEFDE159882D62B2A844A47F86966257D8CACB3A17F42E2CE28A9A464515E9AC9D210FF6F99711B83BE4B2FC2271FD875F7E457E02555E0773EC5DD0EB842D130643C6C1DC178C9146264DFDAF79DFA2CEE3EBE5DAAF6F6208A026498F20AFE9AB84F85EA9F7B9E2F44F23220D79F283DA742E597A60BB589592E43F04A413949BAF8CD46E71B0F441587C4D67E801F7D10D1CDA1BBC40EEAAA02AE885B44F8468F6F119418B0805712EA36A0FBF0286BDE0F1FBFF01217D7FCE7F590000 , N'6.1.2-31219')
END
