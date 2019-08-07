CREATE SEQUENCE public."AspNetRoleClaims_Id_seq";
ALTER SEQUENCE public."AspNetRoleClaims_Id_seq"
    OWNER TO postgres;

CREATE SEQUENCE public."AspNetUserClaims_Id_seq";
ALTER SEQUENCE public."AspNetUserClaims_Id_seq"
    OWNER TO postgres;

CREATE TABLE public."AspNetUsers"
(
    "Id" text COLLATE pg_catalog."default" NOT NULL,
    "UserName" character varying(256) COLLATE pg_catalog."default",
    "NormalizedUserName" character varying(256) COLLATE pg_catalog."default",
    "Email" character varying(256) COLLATE pg_catalog."default",
    "NormalizedEmail" character varying(256) COLLATE pg_catalog."default",
    "EmailConfirmed" boolean NOT NULL,
    "PasswordHash" text COLLATE pg_catalog."default",
    "SecurityStamp" text COLLATE pg_catalog."default",
    "ConcurrencyStamp" text COLLATE pg_catalog."default",
    "PhoneNumber" text COLLATE pg_catalog."default",
    "PhoneNumberConfirmed" boolean NOT NULL,
    "TwoFactorEnabled" boolean NOT NULL,
    "LockoutEnd" timestamp with time zone,
    "LockoutEnabled" boolean NOT NULL,
    "AccessFailedCount" integer NOT NULL,
    "FirstName" character varying(256) COLLATE pg_catalog."default",
    "LastName" character varying(256) COLLATE pg_catalog."default",
    "RegistrationTime" timestamp without time zone NOT NULL DEFAULT timezone('utc'::text, now()),
    "LastUpdateTime" timestamp without time zone NOT NULL DEFAULT timezone('utc'::text, now()),
    "DeleteTime" timestamp without time zone,
    "Deleted" boolean NOT NULL DEFAULT false,
	"PasswordChanged" boolean NOT NULL DEFAULT false,
    CONSTRAINT "PK_AspNetUsers" PRIMARY KEY ("Id")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."AspNetUsers"
    OWNER to postgres;

CREATE INDEX "EmailIndex"
    ON public."AspNetUsers" USING btree
    ("NormalizedEmail" COLLATE pg_catalog."default")
    TABLESPACE pg_default;

CREATE UNIQUE INDEX "UserNameIndex"
    ON public."AspNetUsers" USING btree
    ("NormalizedUserName" COLLATE pg_catalog."default")
    TABLESPACE pg_default;

CREATE TABLE public."AspNetRoles"
(
    "Id" text COLLATE pg_catalog."default" NOT NULL,
    "Name" character varying(256) COLLATE pg_catalog."default",
    "NormalizedName" character varying(256) COLLATE pg_catalog."default",
    "ConcurrencyStamp" text COLLATE pg_catalog."default",
    CONSTRAINT "PK_AspNetRoles" PRIMARY KEY ("Id")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."AspNetRoles"
    OWNER to postgres;

CREATE UNIQUE INDEX "RoleNameIndex"
    ON public."AspNetRoles" USING btree
    ("NormalizedName" COLLATE pg_catalog."default")
    TABLESPACE pg_default;

CREATE TABLE public."AspNetRoleClaims"
(
    "Id" integer NOT NULL DEFAULT nextval('"AspNetRoleClaims_Id_seq"'::regclass),
    "RoleId" text COLLATE pg_catalog."default" NOT NULL,
    "ClaimType" text COLLATE pg_catalog."default",
    "ClaimValue" text COLLATE pg_catalog."default",
    CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId")
        REFERENCES public."AspNetRoles" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."AspNetRoleClaims"
    OWNER to postgres;

CREATE INDEX "IX_AspNetRoleClaims_RoleId"
    ON public."AspNetRoleClaims" USING btree
    ("RoleId" COLLATE pg_catalog."default")
    TABLESPACE pg_default;

CREATE TABLE public."AspNetUserClaims"
(
    "Id" integer NOT NULL DEFAULT nextval('"AspNetUserClaims_Id_seq"'::regclass),
    "UserId" text COLLATE pg_catalog."default" NOT NULL,
    "ClaimType" text COLLATE pg_catalog."default",
    "ClaimValue" text COLLATE pg_catalog."default",
    CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId")
        REFERENCES public."AspNetUsers" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."AspNetUserClaims"
    OWNER to postgres;

CREATE INDEX "IX_AspNetUserClaims_UserId"
    ON public."AspNetUserClaims" USING btree
    ("UserId" COLLATE pg_catalog."default")
    TABLESPACE pg_default;

CREATE TABLE public."AspNetUserLogins"
(
    "LoginProvider" text COLLATE pg_catalog."default" NOT NULL,
    "ProviderKey" text COLLATE pg_catalog."default" NOT NULL,
    "ProviderDisplayName" text COLLATE pg_catalog."default",
    "UserId" text COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId")
        REFERENCES public."AspNetUsers" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."AspNetUserLogins"
    OWNER to postgres;

CREATE INDEX "IX_AspNetUserLogins_UserId"
    ON public."AspNetUserLogins" USING btree
    ("UserId" COLLATE pg_catalog."default")
    TABLESPACE pg_default;

CREATE TABLE public."AspNetUserRoles"
(
    "UserId" text COLLATE pg_catalog."default" NOT NULL,
    "RoleId" text COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId")
        REFERENCES public."AspNetRoles" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE,
    CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId")
        REFERENCES public."AspNetUsers" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."AspNetUserRoles"
    OWNER to postgres;

CREATE INDEX "IX_AspNetUserRoles_RoleId"
    ON public."AspNetUserRoles" USING btree
    ("RoleId" COLLATE pg_catalog."default")
    TABLESPACE pg_default;

CREATE TABLE public."AspNetUserTokens"
(
    "UserId" text COLLATE pg_catalog."default" NOT NULL,
    "LoginProvider" text COLLATE pg_catalog."default" NOT NULL,
    "Name" text COLLATE pg_catalog."default" NOT NULL,
    "Value" text COLLATE pg_catalog."default",
    CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId")
        REFERENCES public."AspNetUsers" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."AspNetUserTokens"
    OWNER to postgres;

CREATE TABLE public."OpenIddictApplications"
(
    "Id" text COLLATE pg_catalog."default" NOT NULL,
    "ClientId" character varying(100) COLLATE pg_catalog."default" NOT NULL,
    "ClientSecret" text COLLATE pg_catalog."default",
    "ConcurrencyToken" character varying(50) COLLATE pg_catalog."default",
    "ConsentType" text COLLATE pg_catalog."default",
    "DisplayName" text COLLATE pg_catalog."default",
    "Permissions" text COLLATE pg_catalog."default",
    "PostLogoutRedirectUris" text COLLATE pg_catalog."default",
    "Properties" text COLLATE pg_catalog."default",
    "RedirectUris" text COLLATE pg_catalog."default",
    "Type" character varying(25) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "PK_OpenIddictApplications" PRIMARY KEY ("Id")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."OpenIddictApplications"
    OWNER to postgres;

CREATE UNIQUE INDEX "IX_OpenIddictApplications_ClientId"
    ON public."OpenIddictApplications" USING btree
    ("ClientId" COLLATE pg_catalog."default")
    TABLESPACE pg_default;

CREATE TABLE public."OpenIddictAuthorizations"
(
    "Id" text COLLATE pg_catalog."default" NOT NULL,
    "ApplicationId" text COLLATE pg_catalog."default",
    "ConcurrencyToken" character varying(50) COLLATE pg_catalog."default",
    "Properties" text COLLATE pg_catalog."default",
    "Scopes" text COLLATE pg_catalog."default",
    "Status" character varying(25) COLLATE pg_catalog."default" NOT NULL,
    "Subject" character varying(450) COLLATE pg_catalog."default" NOT NULL,
    "Type" character varying(25) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "PK_OpenIddictAuthorizations" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_OpenIddictAuthorizations_OpenIddictApplications_Application~" FOREIGN KEY ("ApplicationId")
        REFERENCES public."OpenIddictApplications" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE RESTRICT
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."OpenIddictAuthorizations"
    OWNER to postgres;

CREATE INDEX "IX_OpenIddictAuthorizations_ApplicationId_Status_Subject_Type"
    ON public."OpenIddictAuthorizations" USING btree
    ("ApplicationId" COLLATE pg_catalog."default", "Status" COLLATE pg_catalog."default", "Subject" COLLATE pg_catalog."default", "Type" COLLATE pg_catalog."default")
    TABLESPACE pg_default;

CREATE TABLE public."OpenIddictScopes"
(
    "Id" text COLLATE pg_catalog."default" NOT NULL,
    "ConcurrencyToken" character varying(50) COLLATE pg_catalog."default",
    "Description" text COLLATE pg_catalog."default",
    "DisplayName" text COLLATE pg_catalog."default",
    "Name" character varying(200) COLLATE pg_catalog."default" NOT NULL,
    "Properties" text COLLATE pg_catalog."default",
    "Resources" text COLLATE pg_catalog."default",
    CONSTRAINT "PK_OpenIddictScopes" PRIMARY KEY ("Id")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."OpenIddictScopes"
    OWNER to postgres;

CREATE UNIQUE INDEX "IX_OpenIddictScopes_Name"
    ON public."OpenIddictScopes" USING btree
    ("Name" COLLATE pg_catalog."default")
    TABLESPACE pg_default;

CREATE TABLE public."OpenIddictTokens"
(
    "Id" text COLLATE pg_catalog."default" NOT NULL,
    "ApplicationId" text COLLATE pg_catalog."default",
    "AuthorizationId" text COLLATE pg_catalog."default",
    "ConcurrencyToken" character varying(50) COLLATE pg_catalog."default",
    "CreationDate" timestamp with time zone,
    "ExpirationDate" timestamp with time zone,
    "Payload" text COLLATE pg_catalog."default",
    "Properties" text COLLATE pg_catalog."default",
    "ReferenceId" character varying(100) COLLATE pg_catalog."default",
    "Status" character varying(25) COLLATE pg_catalog."default" NOT NULL,
    "Subject" character varying(450) COLLATE pg_catalog."default" NOT NULL,
    "Type" character varying(25) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "PK_OpenIddictTokens" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_OpenIddictTokens_OpenIddictApplications_ApplicationId" FOREIGN KEY ("ApplicationId")
        REFERENCES public."OpenIddictApplications" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE RESTRICT,
    CONSTRAINT "FK_OpenIddictTokens_OpenIddictAuthorizations_AuthorizationId" FOREIGN KEY ("AuthorizationId")
        REFERENCES public."OpenIddictAuthorizations" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE RESTRICT
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."OpenIddictTokens"
    OWNER to postgres;

CREATE INDEX "IX_OpenIddictTokens_ApplicationId_Status_Subject_Type"
    ON public."OpenIddictTokens" USING btree
    ("ApplicationId" COLLATE pg_catalog."default", "Status" COLLATE pg_catalog."default", "Subject" COLLATE pg_catalog."default", "Type" COLLATE pg_catalog."default")
    TABLESPACE pg_default;

CREATE INDEX "IX_OpenIddictTokens_AuthorizationId"
    ON public."OpenIddictTokens" USING btree
    ("AuthorizationId" COLLATE pg_catalog."default")
    TABLESPACE pg_default;

CREATE UNIQUE INDEX "IX_OpenIddictTokens_ReferenceId"
    ON public."OpenIddictTokens" USING btree
    ("ReferenceId" COLLATE pg_catalog."default")
    TABLESPACE pg_default;
