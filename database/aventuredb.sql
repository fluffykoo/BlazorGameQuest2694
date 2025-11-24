--
-- PostgreSQL database dump
--

\restrict RkAG0x7n8hTdRNFNbhwMzuvVuUGWcXAVi6Lycq7oeqkQLhKuG7pAnMX6ZRQ3QxD

-- Dumped from database version 18.0 (Debian 18.0-1.pgdg13+3)
-- Dumped by pg_dump version 18.0 (Debian 18.0-1.pgdg13+3)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: Administrateurs; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Administrateurs" (
    "Id" uuid NOT NULL,
    "NomUtilisateur" text NOT NULL,
    "Email" text NOT NULL,
    "MotDePasse" text NOT NULL
);


ALTER TABLE public."Administrateurs" OWNER TO postgres;

--
-- Name: Donjons; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Donjons" (
    "Id" uuid NOT NULL,
    "Nom" text NOT NULL,
    "Description" text NOT NULL,
    "NombreDeSalles" integer NOT NULL
);


ALTER TABLE public."Donjons" OWNER TO postgres;

--
-- Name: Joueurs; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Joueurs" (
    "Id" uuid NOT NULL,
    "Nom" text NOT NULL,
    "Mail" text NOT NULL,
    "ScoreTotal" integer NOT NULL,
    "DerniereConnexion" timestamp with time zone NOT NULL,
    "PeutReprendrePartie" boolean NOT NULL,
    "AdministrateurId" uuid
);


ALTER TABLE public."Joueurs" OWNER TO postgres;

--
-- Name: Parties; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Parties" (
    "Id" uuid NOT NULL,
    "JoueurId" uuid NOT NULL,
    "ScoreFinal" integer NOT NULL,
    "Date" timestamp with time zone NOT NULL,
    "EstTerminee" boolean NOT NULL,
    "DonjonId" uuid DEFAULT '00000000-0000-0000-0000-000000000000'::uuid NOT NULL
);


ALTER TABLE public."Parties" OWNER TO postgres;

--
-- Name: Salles; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Salles" (
    "Id" uuid NOT NULL,
    "PartieId" uuid NOT NULL,
    "Position" integer NOT NULL,
    "Description" text NOT NULL,
    "Niveau" integer NOT NULL,
    "ChoixPossible" integer[] NOT NULL,
    "ChoixFait" integer,
    "Resultat_Action" integer,
    "Resultat_Points" integer,
    "Resultat_EstPiege" boolean,
    "Resultat_Message" text,
    "DonjonId" uuid DEFAULT '00000000-0000-0000-0000-000000000000'::uuid,
    "EstVisitee" boolean DEFAULT false NOT NULL,
    "ForceMonstre" integer DEFAULT 0 NOT NULL,
    "ImageMonstre" text,
    "NomMonstre" text,
    "PvMonstre" integer DEFAULT 0 NOT NULL,
    "Resultat_Risque" integer,
    "Resultat_ScoreTotal" integer
);


ALTER TABLE public."Salles" OWNER TO postgres;

--
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- Data for Name: Administrateurs; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Administrateurs" ("Id", "NomUtilisateur", "Email", "MotDePasse") FROM stdin;
1ecac6c9-a878-4892-9f67-e05dcff387bb	admin1	admin1@example.com	motdepasse123
\.


--
-- Data for Name: Donjons; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Donjons" ("Id", "Nom", "Description", "NombreDeSalles") FROM stdin;
ada18a1d-f01f-45b2-97e7-47cfc16873af	Donjon du Feu	Un donjon brûlant rempli de pièges et de monstres.	0
eef8885b-204b-48b7-aef5-777cff74aebf	Donjon aléatoire	Généré automatiquement	5
19a36c49-767b-48f2-9c9d-8c5e01d2e923	Donjon Mystérieux	Généré automatiquement	5
89a8d6df-bae5-4844-9609-35cd46bba053	Donjon Mystérieux	Généré automatiquement	5
0493e456-4bd5-4c9d-85ec-651a78c04dcc	Donjon Mystérieux	Généré automatiquement	5
b4098a9f-deb0-4bb6-87ae-6e245f6fb6ba	Donjon du Feu	Un donjon brûlant rempli de pièges et de monstres.	5
4bb4b157-5457-40ef-8833-b8bcab1cd21d	Donjon de Glace	Une forteresse gelée où le froid est aussi mortel que les ennemis.	4
612f79d3-cdb4-48b5-8029-bd2e57abcfda	Donjon du Temple Ancien	Un ancien temple rempli d’énigmes, de trésors… et de gardiens.	5
3db5fa05-61e7-4829-8a94-07cf5c558958	Donjon de la Forêt Maudite	Une forêt dense et oppressante où chaque arbre semble vous observer.	5
4a90c3f9-165a-4801-ae23-371b4987328e	Donjon du Feu	Un donjon brûlant rempli de pièges et de monstres.	5
8d1b7e8a-fb35-4104-9b99-dcac79810a7e	Donjon de Glace	Une forteresse gelée où le froid est aussi mortel que les ennemis.	3
571f13c4-7f42-454b-9ebd-2f3a817b5735	Donjon des Ombres	Une crypte obscure où rôdent des esprits vengeurs.	5
12d9d724-d8ea-40d6-84b2-b3b32da52cc6	Donjon de la Forêt Maudite	Une forêt dense et oppressante où chaque arbre semble vous observer.	5
9591203c-40aa-4c86-aa14-2563f3375393	Donjon du Temple Ancien	Un ancien temple rempli d’énigmes, de trésors… et de gardiens.	3
eeb62e2b-357c-433d-969a-08dac4c1550c	Donjon du Temple Ancien	Un ancien temple rempli d’énigmes, de trésors… et de gardiens.	5
9ee996f2-daa5-4ca5-b9b1-539e397b8a25	Donjon du Feu	Un donjon brûlant rempli de pièges et de monstres.	5
3de145b0-763f-458b-8164-fc415af95cd0	Donjon de la Forêt Maudite	Une forêt dense et oppressante où chaque arbre semble vous observer.	4
65cec44a-ecfb-4192-89cb-ee6b1797748d	Donjon du Feu	Un donjon brûlant rempli de pièges et de monstres.	4
5429c304-a078-4942-b232-8c77e74bc30f	Donjon des Ombres	Une crypte obscure où rôdent des esprits vengeurs.	5
5ea1d0b4-3bc4-4bfc-a734-a838dfe436a5	Donjon des Ombres	Une crypte obscure où rôdent des esprits vengeurs.	5
0d59e757-ef78-4164-83a8-b1fdfbafc9bb	Donjon de Glace	Une forteresse gelée où le froid est aussi mortel que les ennemis.	4
abcfe7c2-9a0f-4883-8c54-2f951cc6d659	Donjon du Feu	Un donjon brûlant rempli de pièges et de monstres.	4
a830db74-03a7-49c9-9c32-55af73de25fe	Donjon du Feu	Un donjon brûlant rempli de pièges et de monstres.	4
b1817c80-036a-47f7-a428-89123819edcd	Donjon de la Forêt Maudite	Une forêt dense et oppressante où chaque arbre semble vous observer.	4
d6d1df3a-6a6d-4f7a-a72c-adf94d17f4e7	Donjon de Glace	Une forteresse gelée où le froid est aussi mortel que les ennemis.	3
55a4259b-6924-4512-8eb5-6c7d7fbdf1b0	Donjon du Feu	Un donjon brûlant rempli de pièges et de monstres.	4
9c2a7fa3-036a-4e0f-95c9-ef12b2355e48	Donjon de Glace	Une forteresse gelée où le froid est aussi mortel que les ennemis.	5
\.


--
-- Data for Name: Joueurs; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Joueurs" ("Id", "Nom", "Mail", "ScoreTotal", "DerniereConnexion", "PeutReprendrePartie", "AdministrateurId") FROM stdin;
5b4fe318-32dc-4ed7-8cf2-c78ac4c2c74a	Camara	oumou.camara@example.com	0	2025-10-25 13:46:34.065962+00	f	\N
3f9f8a76-d7e9-4642-a3f4-efb0604eab40	Joueur1	joueur1@example.com	0	2025-10-25 18:30:00+00	f	\N
\.


--
-- Data for Name: Parties; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Parties" ("Id", "JoueurId", "ScoreFinal", "Date", "EstTerminee", "DonjonId") FROM stdin;
b082e7de-e6b0-42be-b351-5c94c0040f56	5b4fe318-32dc-4ed7-8cf2-c78ac4c2c74a	0	2025-10-25 16:08:49.712056+00	f	ada18a1d-f01f-45b2-97e7-47cfc16873af
603f1275-ceeb-4d25-b706-8fc48471f15f	3f9f8a76-d7e9-4642-a3f4-efb0604eab40	0	2025-11-23 12:46:57.645695+00	f	19a36c49-767b-48f2-9c9d-8c5e01d2e923
877b2aec-a0ce-44d2-9cab-6587aa1448c0	3f9f8a76-d7e9-4642-a3f4-efb0604eab40	0	2025-11-23 13:00:09.624148+00	f	89a8d6df-bae5-4844-9609-35cd46bba053
eb5fa04b-fdc2-4d91-b8d9-004c21b736b4	3f9f8a76-d7e9-4642-a3f4-efb0604eab40	90	2025-11-23 13:06:28.566715+00	t	0493e456-4bd5-4c9d-85ec-651a78c04dcc
1d539ca1-7a77-4156-8a39-08b7eb1b6ea2	3f9f8a76-d7e9-4642-a3f4-efb0604eab40	0	2025-11-23 14:13:10.023797+00	f	b4098a9f-deb0-4bb6-87ae-6e245f6fb6ba
61b28fa9-a303-4357-997c-69aae858c919	3f9f8a76-d7e9-4642-a3f4-efb0604eab40	0	2025-11-23 14:13:40.98464+00	f	4bb4b157-5457-40ef-8833-b8bcab1cd21d
13e117ac-77fa-4067-93bf-a4390923539f	3f9f8a76-d7e9-4642-a3f4-efb0604eab40	130	2025-11-23 14:13:58.713469+00	t	612f79d3-cdb4-48b5-8029-bd2e57abcfda
6a08e88d-0948-425f-b744-20d9cc33895a	3f9f8a76-d7e9-4642-a3f4-efb0604eab40	0	2025-11-23 14:35:11.494935+00	f	3db5fa05-61e7-4829-8a94-07cf5c558958
bb2b2243-0586-4d7c-89ec-91caeb1a2da7	3f9f8a76-d7e9-4642-a3f4-efb0604eab40	0	2025-11-23 14:35:29.759271+00	f	4a90c3f9-165a-4801-ae23-371b4987328e
cc8b4ae3-c4a1-471f-945c-10f11e647452	3f9f8a76-d7e9-4642-a3f4-efb0604eab40	0	2025-11-23 14:46:31.032092+00	f	8d1b7e8a-fb35-4104-9b99-dcac79810a7e
be93a3f2-73ad-4901-b1a6-a85173b299c5	3f9f8a76-d7e9-4642-a3f4-efb0604eab40	0	2025-11-23 14:53:44.128236+00	f	571f13c4-7f42-454b-9ebd-2f3a817b5735
ad86fdf7-8c7b-4095-8523-c9c39074ee6b	3f9f8a76-d7e9-4642-a3f4-efb0604eab40	0	2025-11-23 15:01:39.384407+00	f	12d9d724-d8ea-40d6-84b2-b3b32da52cc6
1a08b75d-45c5-4006-af46-0d49bf6c7b35	3f9f8a76-d7e9-4642-a3f4-efb0604eab40	10	2025-11-23 15:25:44.340496+00	f	9591203c-40aa-4c86-aa14-2563f3375393
e8bc4158-65e3-4c7b-914e-200ee8cde15e	3f9f8a76-d7e9-4642-a3f4-efb0604eab40	0	2025-11-23 15:49:34.944594+00	f	eeb62e2b-357c-433d-969a-08dac4c1550c
2f552356-934f-4805-b144-6a56d9554b67	3f9f8a76-d7e9-4642-a3f4-efb0604eab40	50	2025-11-23 16:23:45.740789+00	t	9ee996f2-daa5-4ca5-b9b1-539e397b8a25
04e4e161-a193-40b8-ba37-cf2772f06fe0	3f9f8a76-d7e9-4642-a3f4-efb0604eab40	0	2025-11-23 16:27:47.465798+00	f	3de145b0-763f-458b-8164-fc415af95cd0
333836ab-1cf9-4a1e-a2a1-1e9e75d18ef9	3f9f8a76-d7e9-4642-a3f4-efb0604eab40	10	2025-11-23 16:41:44.769855+00	f	65cec44a-ecfb-4192-89cb-ee6b1797748d
44c216d5-99e0-4a79-8a39-483175e2d041	3f9f8a76-d7e9-4642-a3f4-efb0604eab40	0	2025-11-23 17:13:38.720823+00	f	5429c304-a078-4942-b232-8c77e74bc30f
b820c5e7-b609-48b2-bbbf-d0476ce9980f	3f9f8a76-d7e9-4642-a3f4-efb0604eab40	0	2025-11-23 17:13:41.300261+00	f	5ea1d0b4-3bc4-4bfc-a734-a838dfe436a5
caf4ee17-2606-45a5-8a62-e8536677c69c	3f9f8a76-d7e9-4642-a3f4-efb0604eab40	0	2025-11-23 17:13:42.56985+00	f	0d59e757-ef78-4164-83a8-b1fdfbafc9bb
2f83fc20-4b0c-4519-a057-bc0e33ba0882	3f9f8a76-d7e9-4642-a3f4-efb0604eab40	0	2025-11-23 17:21:06.439159+00	f	abcfe7c2-9a0f-4883-8c54-2f951cc6d659
fa0f2fb4-877b-4ce2-992b-fcbf922defe4	3f9f8a76-d7e9-4642-a3f4-efb0604eab40	0	2025-11-23 17:21:08.421045+00	f	a830db74-03a7-49c9-9c32-55af73de25fe
aa0eef7c-a432-41c8-9769-ac5896b99ef9	3f9f8a76-d7e9-4642-a3f4-efb0604eab40	0	2025-11-23 17:21:09.561528+00	f	b1817c80-036a-47f7-a428-89123819edcd
17f8be0b-1a2b-474d-94fc-c991ade5a8a8	3f9f8a76-d7e9-4642-a3f4-efb0604eab40	50	2025-11-23 17:25:04.027837+00	t	d6d1df3a-6a6d-4f7a-a72c-adf94d17f4e7
4784d565-adc3-4246-90e0-eba3c2aceaa0	3f9f8a76-d7e9-4642-a3f4-efb0604eab40	40	2025-11-23 23:14:12.485223+00	t	9c2a7fa3-036a-4e0f-95c9-ef12b2355e48
c119c61b-11a5-4fb2-9594-e06bb998afc1	3f9f8a76-d7e9-4642-a3f4-efb0604eab40	10	2025-11-23 17:26:33.291353+00	t	55a4259b-6924-4512-8eb5-6c7d7fbdf1b0
\.


--
-- Data for Name: Salles; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Salles" ("Id", "PartieId", "Position", "Description", "Niveau", "ChoixPossible", "ChoixFait", "Resultat_Action", "Resultat_Points", "Resultat_EstPiege", "Resultat_Message", "DonjonId", "EstVisitee", "ForceMonstre", "ImageMonstre", "NomMonstre", "PvMonstre", "Resultat_Risque", "Resultat_ScoreTotal") FROM stdin;
c15f7b51-9d72-43a6-aac0-26af5ddf5796	b082e7de-e6b0-42be-b351-5c94c0040f56	1	Une salle humide et glaciale, avec un bruit de chaînes au loin...	1	{0,1,2}	0	0	15	f	Tu avances prudemment, sans déclencher de piège.	ada18a1d-f01f-45b2-97e7-47cfc16873af	f	0	\N	\N	0	\N	\N
56e002b1-84b7-4b75-ab7c-c822af9a87ab	603f1275-ceeb-4d25-b706-8fc48471f15f	4	Salle 4 : un Gobelin vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	19a36c49-767b-48f2-9c9d-8c5e01d2e923	f	8	goblin.png	Gobelin	40	\N	\N
88b416bf-9347-4473-95f6-71ac97e5b49e	603f1275-ceeb-4d25-b706-8fc48471f15f	5	Salle 5 : un Dragonnet vous attend...	2	{0,2,1}	\N	\N	\N	\N	\N	19a36c49-767b-48f2-9c9d-8c5e01d2e923	f	10	dragon.png	Dragonnet	50	\N	\N
c46bcf50-7036-4271-b027-505470339a4d	603f1275-ceeb-4d25-b706-8fc48471f15f	2	Salle 2 : un Orc vous attend...	0	{0,2,1}	\N	\N	\N	\N	\N	19a36c49-767b-48f2-9c9d-8c5e01d2e923	f	4	orc.png	Orc	20	\N	\N
d05923c6-d899-45f9-ac6c-04fd351596cc	603f1275-ceeb-4d25-b706-8fc48471f15f	1	Salle 1 : un Dragonnet vous attend...	0	{0,2,1}	\N	\N	\N	\N	\N	19a36c49-767b-48f2-9c9d-8c5e01d2e923	f	2	dragon.png	Dragonnet	10	\N	\N
f6b2038d-3001-4037-b305-d9f89a30e7f0	603f1275-ceeb-4d25-b706-8fc48471f15f	3	Salle 3 : un Dragonnet vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	19a36c49-767b-48f2-9c9d-8c5e01d2e923	f	6	dragon.png	Dragonnet	30	\N	\N
1a3485e7-7ec2-4a00-82ac-691e50267e7e	877b2aec-a0ce-44d2-9cab-6587aa1448c0	3	Salle 3 : un Gobelin vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	89a8d6df-bae5-4844-9609-35cd46bba053	f	6	goblin.png	Gobelin	30	\N	\N
3b45c007-89da-4966-8049-70c7696f1b73	877b2aec-a0ce-44d2-9cab-6587aa1448c0	1	Salle 1 : un Dragonnet vous attend...	0	{0,2,1}	\N	\N	\N	\N	\N	89a8d6df-bae5-4844-9609-35cd46bba053	f	2	dragon.png	Dragonnet	10	\N	\N
b4c67864-5ac5-4bf8-96f3-345e14bb46c5	877b2aec-a0ce-44d2-9cab-6587aa1448c0	4	Salle 4 : un Orc vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	89a8d6df-bae5-4844-9609-35cd46bba053	f	8	orc.png	Orc	40	\N	\N
d0130d22-41a2-4cfd-a347-46f97821079f	877b2aec-a0ce-44d2-9cab-6587aa1448c0	5	Salle 5 : un Gobelin vous attend...	2	{0,2,1}	\N	\N	\N	\N	\N	89a8d6df-bae5-4844-9609-35cd46bba053	f	10	goblin.png	Gobelin	50	\N	\N
f28b8eb3-2c79-4a93-8166-a3e9353392c1	877b2aec-a0ce-44d2-9cab-6587aa1448c0	2	Salle 2 : un Orc vous attend...	0	{0,2,1}	\N	\N	\N	\N	\N	89a8d6df-bae5-4844-9609-35cd46bba053	f	4	orc.png	Orc	20	\N	\N
78ded94d-c7ed-4dda-b49c-d4253e989563	eb5fa04b-fdc2-4d91-b8d9-004c21b736b4	1	Salle 1 : un Dragonnet vous attend...	0	{0,2,1}	0	0	0	f	Victoire ! Vous avez terrassé le Dragonnet.	0493e456-4bd5-4c9d-85ec-651a78c04dcc	t	2	dragon.png	Dragonnet	0	\N	\N
8937f9d6-0508-4b96-b4f7-aaba03bbf879	eb5fa04b-fdc2-4d91-b8d9-004c21b736b4	2	Salle 2 : un Dragonnet vous attend...	0	{0,2,1}	1	1	10	f	Vous avez fui lâchement mais vous êtes en vie.	0493e456-4bd5-4c9d-85ec-651a78c04dcc	t	4	dragon.png	Dragonnet	20	\N	\N
7027d43b-6196-450e-9b21-91bbba7dcd4f	eb5fa04b-fdc2-4d91-b8d9-004c21b736b4	3	Salle 3 : un Dragonnet vous attend...	1	{0,2,1}	2	2	30	f	Vous avez trouvé une potion rare !	0493e456-4bd5-4c9d-85ec-651a78c04dcc	t	6	dragon.png	Dragonnet	30	\N	\N
c2487629-8cce-4fb6-b08b-57c56e63b724	eb5fa04b-fdc2-4d91-b8d9-004c21b736b4	4	Salle 4 : un Gobelin vous attend...	1	{0,2,1}	0	0	50	f	Victoire ! Vous avez terrassé le Gobelin.	0493e456-4bd5-4c9d-85ec-651a78c04dcc	t	8	goblin.png	Gobelin	0	\N	\N
4fb0b955-dc13-4f2d-9980-2831efe9de7f	eb5fa04b-fdc2-4d91-b8d9-004c21b736b4	5	Salle 5 : un Squelette vous attend...	2	{0,2,1}	0	0	0	f	Échec... Le Squelette vous a blessé. (FIN DU DONJON)	0493e456-4bd5-4c9d-85ec-651a78c04dcc	t	10	skeleton.png	Squelette	50	\N	\N
248aae9c-1e4c-459c-8183-ba9ea73d2eab	1d539ca1-7a77-4156-8a39-08b7eb1b6ea2	1	Salle 1 : un Dragonnet vous attend...	0	{0,2,1}	\N	\N	\N	\N	\N	b4098a9f-deb0-4bb6-87ae-6e245f6fb6ba	f	2	dragon.png	Dragonnet	10	\N	\N
2a0cbdc7-739e-4a1a-8c22-8fbdd671b306	1d539ca1-7a77-4156-8a39-08b7eb1b6ea2	4	Salle 4 : un Dragonnet vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	b4098a9f-deb0-4bb6-87ae-6e245f6fb6ba	f	8	dragon.png	Dragonnet	40	\N	\N
2e28df2e-0db2-42ee-b689-46b504527fc3	1d539ca1-7a77-4156-8a39-08b7eb1b6ea2	5	Salle 5 : un Orc vous attend...	2	{0,2,1}	\N	\N	\N	\N	\N	b4098a9f-deb0-4bb6-87ae-6e245f6fb6ba	f	10	orc.png	Orc	50	\N	\N
4350128c-698f-4de0-978f-c48b95bff880	1d539ca1-7a77-4156-8a39-08b7eb1b6ea2	3	Salle 3 : un Squelette vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	b4098a9f-deb0-4bb6-87ae-6e245f6fb6ba	f	6	skeleton.png	Squelette	30	\N	\N
44718aff-ba58-410f-b6d6-254c6cdbc662	1d539ca1-7a77-4156-8a39-08b7eb1b6ea2	2	Salle 2 : un Squelette vous attend...	0	{0,2,1}	\N	\N	\N	\N	\N	b4098a9f-deb0-4bb6-87ae-6e245f6fb6ba	f	4	skeleton.png	Squelette	20	\N	\N
84190b45-6f3f-4ab2-abf1-d7877292ab1d	61b28fa9-a303-4357-997c-69aae858c919	2	Salle 2 : un Dragonnet vous attend...	0	{0,2,1}	\N	\N	\N	\N	\N	4bb4b157-5457-40ef-8833-b8bcab1cd21d	f	4	dragon.png	Dragonnet	20	\N	\N
ce6c5309-6407-4f1c-b84e-ff4de532c655	61b28fa9-a303-4357-997c-69aae858c919	1	Salle 1 : un Dragonnet vous attend...	0	{0,2,1}	\N	\N	\N	\N	\N	4bb4b157-5457-40ef-8833-b8bcab1cd21d	f	2	dragon.png	Dragonnet	10	\N	\N
d1cddd9b-deee-4c29-8f51-71baa9d005e6	61b28fa9-a303-4357-997c-69aae858c919	3	Salle 3 : un Orc vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	4bb4b157-5457-40ef-8833-b8bcab1cd21d	f	6	orc.png	Orc	30	\N	\N
d76fc7ed-32e4-4320-8612-cf782905fd7b	61b28fa9-a303-4357-997c-69aae858c919	4	Salle 4 : un Gobelin vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	4bb4b157-5457-40ef-8833-b8bcab1cd21d	f	8	goblin.png	Gobelin	40	\N	\N
177fbcf8-3e0f-41e3-ac2f-4b5dd8986559	13e117ac-77fa-4067-93bf-a4390923539f	1	Salle 1 : un Dragonnet vous attend...	0	{0,2,1}	2	2	30	f	Vous avez trouvé une potion rare !	612f79d3-cdb4-48b5-8029-bd2e57abcfda	t	2	dragon.png	Dragonnet	10	\N	\N
de1028f2-a489-444a-a238-ecb216ee6c4e	13e117ac-77fa-4067-93bf-a4390923539f	2	Salle 2 : un Dragonnet vous attend...	0	{0,2,1}	0	0	0	f	Victoire ! Vous avez terrassé le Dragonnet.	612f79d3-cdb4-48b5-8029-bd2e57abcfda	t	4	dragon.png	Dragonnet	0	\N	\N
02f2865f-3dd2-47e5-b4ee-04889d5d15d1	13e117ac-77fa-4067-93bf-a4390923539f	3	Salle 3 : un Orc vous attend...	1	{0,2,1}	0	0	0	f	Échec... Le Orc vous a blessé.	612f79d3-cdb4-48b5-8029-bd2e57abcfda	t	6	orc.png	Orc	30	\N	\N
3a4a1d35-d049-4351-a1c8-53a5607a009c	13e117ac-77fa-4067-93bf-a4390923539f	4	Salle 4 : un Squelette vous attend...	1	{0,2,1}	0	0	0	f	Échec... Le Squelette vous a blessé.	612f79d3-cdb4-48b5-8029-bd2e57abcfda	t	8	skeleton.png	Squelette	40	\N	\N
d13dbb45-15fb-416b-a58d-3d18fb9fd0c6	13e117ac-77fa-4067-93bf-a4390923539f	5	Salle 5 : un Squelette vous attend...	2	{0,2,1}	0	0	100	f	Victoire ! Vous avez terrassé le Squelette. (FIN DU DONJON)	612f79d3-cdb4-48b5-8029-bd2e57abcfda	t	10	skeleton.png	Squelette	0	\N	\N
5530b600-f0fc-40e3-8cd3-4d6988096718	6a08e88d-0948-425f-b744-20d9cc33895a	1	Salle 1 : un Dragonnet vous attend...	0	{0,2,1}	\N	\N	\N	\N	\N	3db5fa05-61e7-4829-8a94-07cf5c558958	f	2	dragon.png	Dragonnet	10	\N	\N
6f382f91-0efd-40ef-a915-40ee3ced77c6	6a08e88d-0948-425f-b744-20d9cc33895a	4	Salle 4 : un Orc vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	3db5fa05-61e7-4829-8a94-07cf5c558958	f	8	orc.png	Orc	40	\N	\N
b249d2cc-d2c6-4edb-9426-05042ed4f695	6a08e88d-0948-425f-b744-20d9cc33895a	5	Salle 5 : un Dragonnet vous attend...	2	{0,2,1}	\N	\N	\N	\N	\N	3db5fa05-61e7-4829-8a94-07cf5c558958	f	10	dragon.png	Dragonnet	50	\N	\N
ebb13b94-a00d-4af7-9077-e784c1080f86	6a08e88d-0948-425f-b744-20d9cc33895a	2	Salle 2 : un Dragonnet vous attend...	0	{0,2,1}	\N	\N	\N	\N	\N	3db5fa05-61e7-4829-8a94-07cf5c558958	f	4	dragon.png	Dragonnet	20	\N	\N
ee57a930-810e-4d9a-93ee-5e55a64c937e	6a08e88d-0948-425f-b744-20d9cc33895a	3	Salle 3 : un Orc vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	3db5fa05-61e7-4829-8a94-07cf5c558958	f	6	orc.png	Orc	30	\N	\N
5256e3e5-2a60-4437-a4c7-2f85abffce03	bb2b2243-0586-4d7c-89ec-91caeb1a2da7	4	Salle 4 : un Dragonnet vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	4a90c3f9-165a-4801-ae23-371b4987328e	f	8	dragon.png	Dragonnet	40	\N	\N
80dbe6a8-46ca-4240-bf2f-81798e3c440e	bb2b2243-0586-4d7c-89ec-91caeb1a2da7	1	Salle 1 : un Dragonnet vous attend...	0	{0,2,1}	\N	\N	\N	\N	\N	4a90c3f9-165a-4801-ae23-371b4987328e	f	2	dragon.png	Dragonnet	10	\N	\N
ba2ed24d-7b7c-445f-b96c-76fc62bef15b	bb2b2243-0586-4d7c-89ec-91caeb1a2da7	2	Salle 2 : un Gobelin vous attend...	0	{0,2,1}	\N	\N	\N	\N	\N	4a90c3f9-165a-4801-ae23-371b4987328e	f	4	goblin.png	Gobelin	20	\N	\N
c4c56635-151e-40dd-9902-23a1a4b84154	bb2b2243-0586-4d7c-89ec-91caeb1a2da7	3	Salle 3 : un Squelette vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	4a90c3f9-165a-4801-ae23-371b4987328e	f	6	skeleton.png	Squelette	30	\N	\N
d2883b04-e2d5-475c-bd8c-ceffccf505ae	bb2b2243-0586-4d7c-89ec-91caeb1a2da7	5	Salle 5 : un Dragonnet vous attend...	2	{0,2,1}	\N	\N	\N	\N	\N	4a90c3f9-165a-4801-ae23-371b4987328e	f	10	dragon.png	Dragonnet	50	\N	\N
10451bc8-5c35-42c9-b6e9-f7971e062807	cc8b4ae3-c4a1-471f-945c-10f11e647452	2	Salle 2 : un Squelette vous attend...	0	{0,2,1}	\N	\N	\N	\N	\N	8d1b7e8a-fb35-4104-9b99-dcac79810a7e	f	4	skeleton.png	Squelette	20	\N	\N
88b8f039-ab04-4d13-ba18-9b5715473557	cc8b4ae3-c4a1-471f-945c-10f11e647452	3	Salle 3 : un Orc vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	8d1b7e8a-fb35-4104-9b99-dcac79810a7e	f	6	orc.png	Orc	30	\N	\N
d1622726-eae9-4777-bdcd-44a4f7987e0e	cc8b4ae3-c4a1-471f-945c-10f11e647452	1	Salle 1 : un Dragonnet vous attend...	0	{0,2,1}	\N	\N	\N	\N	\N	8d1b7e8a-fb35-4104-9b99-dcac79810a7e	f	2	dragon.png	Dragonnet	10	\N	\N
1754239c-3985-4837-9eaf-42d77f48c944	be93a3f2-73ad-4901-b1a6-a85173b299c5	3	Salle 3 : un Squelette vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	571f13c4-7f42-454b-9ebd-2f3a817b5735	f	6	skeleton.png	Squelette	30	\N	\N
3c585a7c-f401-4fa9-a0fe-e505bd63588d	be93a3f2-73ad-4901-b1a6-a85173b299c5	5	Salle 5 : un Dragonnet vous attend...	2	{0,2,1}	\N	\N	\N	\N	\N	571f13c4-7f42-454b-9ebd-2f3a817b5735	f	10	dragon.png	Dragonnet	50	\N	\N
56bd1fbe-1580-4316-9134-9ced39b2be8d	be93a3f2-73ad-4901-b1a6-a85173b299c5	4	Salle 4 : un Orc vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	571f13c4-7f42-454b-9ebd-2f3a817b5735	f	8	orc.png	Orc	40	\N	\N
6638973a-f931-4672-923b-18a23a99062f	be93a3f2-73ad-4901-b1a6-a85173b299c5	2	Salle 2 : un Squelette vous attend...	0	{0,2,1}	\N	\N	\N	\N	\N	571f13c4-7f42-454b-9ebd-2f3a817b5735	f	4	skeleton.png	Squelette	20	\N	\N
789c824f-fea7-47b7-8fab-a23b630df209	be93a3f2-73ad-4901-b1a6-a85173b299c5	1	Salle 1 : un Dragonnet vous attend...	0	{0,2,1}	\N	\N	\N	\N	\N	571f13c4-7f42-454b-9ebd-2f3a817b5735	f	2	dragon.png	Dragonnet	10	\N	\N
6b61510b-6cdb-49a7-977e-dc7a197b755f	ad86fdf7-8c7b-4095-8523-c9c39074ee6b	4	Salle 4 : un Orc vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	12d9d724-d8ea-40d6-84b2-b3b32da52cc6	f	8	orc.png	Orc	40	\N	\N
7bec5c16-f467-4c81-93dd-7dad9d7fa289	ad86fdf7-8c7b-4095-8523-c9c39074ee6b	5	Salle 5 : un Squelette vous attend...	2	{0,2,1}	\N	\N	\N	\N	\N	12d9d724-d8ea-40d6-84b2-b3b32da52cc6	f	10	skeleton.png	Squelette	50	\N	\N
7f5ddbee-bc08-4d96-b50a-6e3139374bb7	ad86fdf7-8c7b-4095-8523-c9c39074ee6b	2	Salle 2 : un Squelette vous attend...	0	{0,2,1}	\N	\N	\N	\N	\N	12d9d724-d8ea-40d6-84b2-b3b32da52cc6	f	4	skeleton.png	Squelette	20	\N	\N
e80e6cbc-a331-4c65-85c6-72ae466d8d9a	ad86fdf7-8c7b-4095-8523-c9c39074ee6b	3	Salle 3 : un Dragonnet vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	12d9d724-d8ea-40d6-84b2-b3b32da52cc6	f	6	dragon.png	Dragonnet	30	\N	\N
fdb522ee-2664-49eb-86f3-61336558714e	ad86fdf7-8c7b-4095-8523-c9c39074ee6b	1	Salle 1 : un Orc vous attend...	0	{0,2,1}	\N	\N	\N	\N	\N	12d9d724-d8ea-40d6-84b2-b3b32da52cc6	f	2	orc.png	Orc	10	\N	\N
d1f59f47-a6cc-4518-9a73-ad7f4620a3e9	1a08b75d-45c5-4006-af46-0d49bf6c7b35	1	Salle 1 : un Squelette vous attend...	0	{0,2,1}	0	0	0	f	Victoire ! Vous avez terrassé le Squelette.	9591203c-40aa-4c86-aa14-2563f3375393	t	2	skeleton.png	Squelette	0	\N	\N
e746c545-25e3-48b0-80e9-ed7150a4e69f	1a08b75d-45c5-4006-af46-0d49bf6c7b35	2	Salle 2 : un Dragonnet vous attend...	0	{0,2,1}	0	0	0	f	Victoire ! Vous avez terrassé le Dragonnet.	9591203c-40aa-4c86-aa14-2563f3375393	t	4	dragon.png	Dragonnet	0	\N	\N
5799918d-2c16-4707-86a3-3ad3136566cd	1a08b75d-45c5-4006-af46-0d49bf6c7b35	3	Salle 3 : un Orc vous attend...	1	{0,2,1}	1	1	10	f	Vous avez fui lâchement mais vous êtes en vie.	9591203c-40aa-4c86-aa14-2563f3375393	t	6	orc.png	Orc	30	\N	\N
461941d9-9275-4da8-81da-1e3fcdbea007	e8bc4158-65e3-4c7b-914e-200ee8cde15e	1	Salle 1 : un Orc vous attend...	0	{0,2,1}	\N	\N	\N	\N	\N	eeb62e2b-357c-433d-969a-08dac4c1550c	f	2	orc.png	Orc	10	\N	\N
651f8032-18d8-4f36-8c3f-6fd99439ac95	e8bc4158-65e3-4c7b-914e-200ee8cde15e	5	Salle 5 : un Orc vous attend...	2	{0,2,1}	\N	\N	\N	\N	\N	eeb62e2b-357c-433d-969a-08dac4c1550c	f	10	orc.png	Orc	50	\N	\N
7f695904-7968-4b92-baea-cdf0f75f720a	e8bc4158-65e3-4c7b-914e-200ee8cde15e	3	Salle 3 : un Dragonnet vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	eeb62e2b-357c-433d-969a-08dac4c1550c	f	6	dragon.png	Dragonnet	30	\N	\N
96f90361-68c3-4a25-afa6-1f10d1efa625	e8bc4158-65e3-4c7b-914e-200ee8cde15e	4	Salle 4 : un Dragonnet vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	eeb62e2b-357c-433d-969a-08dac4c1550c	f	8	dragon.png	Dragonnet	40	\N	\N
c2398a82-9262-499f-b4ea-c4a4fdf6061a	e8bc4158-65e3-4c7b-914e-200ee8cde15e	2	Salle 2 : un Dragonnet vous attend...	0	{0,2,1}	\N	\N	\N	\N	\N	eeb62e2b-357c-433d-969a-08dac4c1550c	f	4	dragon.png	Dragonnet	20	\N	\N
f4f1e26d-12e6-4e5f-b8f9-47e42f079d98	2f552356-934f-4805-b144-6a56d9554b67	1	Salle 1 : un Orc vous attend...	0	{0,2,1}	0	0	0	f	Victoire ! Vous avez terrassé le Orc.	9ee996f2-daa5-4ca5-b9b1-539e397b8a25	t	2	orc.png	Orc	0	\N	\N
2b31bd2f-1c17-4876-ae03-6abd1a5e83b6	2f552356-934f-4805-b144-6a56d9554b67	2	Salle 2 : un Squelette vous attend...	0	{0,2,1}	0	0	0	f	Victoire ! Vous avez terrassé le Squelette.	9ee996f2-daa5-4ca5-b9b1-539e397b8a25	t	4	skeleton.png	Squelette	0	\N	\N
dfb81154-8f44-4122-b058-c656dd57643b	2f552356-934f-4805-b144-6a56d9554b67	3	Salle 3 : un Orc vous attend...	1	{0,2,1}	0	0	50	f	Victoire ! Vous avez terrassé le Orc.	9ee996f2-daa5-4ca5-b9b1-539e397b8a25	t	6	orc.png	Orc	0	\N	\N
658d60bd-c59e-4623-a73c-ecf1a73757a6	2f552356-934f-4805-b144-6a56d9554b67	4	Salle 4 : un Dragonnet vous attend...	1	{0,2,1}	2	2	-10	t	C'était un piège ! Vous perdez des PV.	9ee996f2-daa5-4ca5-b9b1-539e397b8a25	t	8	dragon.png	Dragonnet	40	\N	\N
81079de0-cff2-4156-9445-4cdf9ef3e1d9	2f552356-934f-4805-b144-6a56d9554b67	5	Salle 5 : un Squelette vous attend...	2	{0,2,1}	1	1	10	f	Vous avez fui lâchement mais vous êtes en vie. (FIN DU DONJON)	9ee996f2-daa5-4ca5-b9b1-539e397b8a25	t	10	skeleton.png	Squelette	50	\N	\N
0dccc7bb-a836-4875-a8dd-8ea2d10957c3	04e4e161-a193-40b8-ba37-cf2772f06fe0	3	Salle 3 : un Gobelin vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	3de145b0-763f-458b-8164-fc415af95cd0	f	6	goblin.png	Gobelin	30	\N	\N
59a4e2ff-232b-4277-9589-3c4edab2f21a	04e4e161-a193-40b8-ba37-cf2772f06fe0	2	Salle 2 : un Squelette vous attend...	0	{0,2,1}	\N	\N	\N	\N	\N	3de145b0-763f-458b-8164-fc415af95cd0	f	4	skeleton.png	Squelette	20	\N	\N
dc6ff924-2ba9-4c52-a512-5ed24e316219	04e4e161-a193-40b8-ba37-cf2772f06fe0	1	Salle 1 : un Dragonnet vous attend...	0	{0,2,1}	\N	\N	\N	\N	\N	3de145b0-763f-458b-8164-fc415af95cd0	f	2	dragon.png	Dragonnet	10	\N	\N
e969cbc4-ba33-4374-b51f-8b2b11a2c05a	04e4e161-a193-40b8-ba37-cf2772f06fe0	4	Salle 4 : un Squelette vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	3de145b0-763f-458b-8164-fc415af95cd0	f	8	skeleton.png	Squelette	40	\N	\N
42448905-36bb-4d62-84b9-57fb8f8beb00	333836ab-1cf9-4a1e-a2a1-1e9e75d18ef9	3	Salle 3 : un Orc vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	65cec44a-ecfb-4192-89cb-ee6b1797748d	f	6	orc.png	Orc	30	\N	\N
8b03e63a-039c-413b-8aab-f9e2a989f722	333836ab-1cf9-4a1e-a2a1-1e9e75d18ef9	4	Salle 4 : un Squelette vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	65cec44a-ecfb-4192-89cb-ee6b1797748d	f	8	skeleton.png	Squelette	40	\N	\N
15e3d700-26b6-4f3b-8d5a-5e8a4914d29e	333836ab-1cf9-4a1e-a2a1-1e9e75d18ef9	1	Salle 1 : un Dragonnet vous attend...	0	{0,2,1}	0	0	0	f	Victoire ! Vous avez terrassé le Dragonnet.	65cec44a-ecfb-4192-89cb-ee6b1797748d	t	2	dragon.png	Dragonnet	0	\N	\N
f6377678-a78c-45f5-bb5d-b8c12681e649	333836ab-1cf9-4a1e-a2a1-1e9e75d18ef9	2	Salle 2 : un Squelette vous attend...	0	{0,2,1}	1	1	10	f	Vous avez fui lâchement mais vous êtes en vie.	65cec44a-ecfb-4192-89cb-ee6b1797748d	t	4	skeleton.png	Squelette	20	\N	\N
04dff4c2-f945-4725-9f3e-8ee4e2315de5	44c216d5-99e0-4a79-8a39-483175e2d041	2	Salle 2 : un Dragonnet vous attend...	0	{0,2,1}	\N	\N	\N	\N	\N	5429c304-a078-4942-b232-8c77e74bc30f	f	4	dragon.png	Dragonnet	20	\N	\N
55526298-3f4a-47d8-bb2e-511daf8f72bd	44c216d5-99e0-4a79-8a39-483175e2d041	4	Salle 4 : un Orc vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	5429c304-a078-4942-b232-8c77e74bc30f	f	8	orc.png	Orc	40	\N	\N
bfb69aad-4dac-45a5-b732-1f31b2f1c928	44c216d5-99e0-4a79-8a39-483175e2d041	3	Salle 3 : un Orc vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	5429c304-a078-4942-b232-8c77e74bc30f	f	6	orc.png	Orc	30	\N	\N
d2a54347-5c16-4aed-8007-87e360c995ba	44c216d5-99e0-4a79-8a39-483175e2d041	5	Salle 5 : un Gobelin vous attend...	2	{0,2,1}	\N	\N	\N	\N	\N	5429c304-a078-4942-b232-8c77e74bc30f	f	10	goblin.png	Gobelin	50	\N	\N
ea716e09-3d70-44b1-b836-f9fd2c8f0e65	44c216d5-99e0-4a79-8a39-483175e2d041	1	Salle 1 : un Squelette vous attend...	0	{0,2,1}	\N	\N	\N	\N	\N	5429c304-a078-4942-b232-8c77e74bc30f	f	2	skeleton.png	Squelette	10	\N	\N
37d99cbf-f11e-463f-b753-abb71de3ba5b	b820c5e7-b609-48b2-bbbf-d0476ce9980f	3	Salle 3 : un Gobelin vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	5ea1d0b4-3bc4-4bfc-a734-a838dfe436a5	f	6	goblin.png	Gobelin	30	\N	\N
7237ba9a-49d3-43ea-9a4d-7b1c33afcd83	b820c5e7-b609-48b2-bbbf-d0476ce9980f	1	Salle 1 : un Gobelin vous attend...	0	{0,2,1}	\N	\N	\N	\N	\N	5ea1d0b4-3bc4-4bfc-a734-a838dfe436a5	f	2	goblin.png	Gobelin	10	\N	\N
845d1f59-0484-4076-8233-666dc11c620e	b820c5e7-b609-48b2-bbbf-d0476ce9980f	2	Salle 2 : un Dragonnet vous attend...	0	{0,2,1}	\N	\N	\N	\N	\N	5ea1d0b4-3bc4-4bfc-a734-a838dfe436a5	f	4	dragon.png	Dragonnet	20	\N	\N
8be2bd38-22f1-4dfe-acc1-735f7aeb2867	b820c5e7-b609-48b2-bbbf-d0476ce9980f	5	Salle 5 : un Orc vous attend...	2	{0,2,1}	\N	\N	\N	\N	\N	5ea1d0b4-3bc4-4bfc-a734-a838dfe436a5	f	10	orc.png	Orc	50	\N	\N
a7037811-eb42-44aa-9f83-1a932a718517	b820c5e7-b609-48b2-bbbf-d0476ce9980f	4	Salle 4 : un Dragonnet vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	5ea1d0b4-3bc4-4bfc-a734-a838dfe436a5	f	8	dragon.png	Dragonnet	40	\N	\N
158d28ef-cffa-4809-8e46-9f38cde44cc5	caf4ee17-2606-45a5-8a62-e8536677c69c	2	Salle 2 : un Orc vous attend...	0	{0,2,1}	\N	\N	\N	\N	\N	0d59e757-ef78-4164-83a8-b1fdfbafc9bb	f	4	orc.png	Orc	20	\N	\N
5abdd61f-9bce-4e22-a208-8c065d7e8178	caf4ee17-2606-45a5-8a62-e8536677c69c	4	Salle 4 : un Orc vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	0d59e757-ef78-4164-83a8-b1fdfbafc9bb	f	8	orc.png	Orc	40	\N	\N
9fed40cd-fdf4-4af9-9adb-e38eb7d86dc3	caf4ee17-2606-45a5-8a62-e8536677c69c	3	Salle 3 : un Dragonnet vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	0d59e757-ef78-4164-83a8-b1fdfbafc9bb	f	6	dragon.png	Dragonnet	30	\N	\N
f4eb8ee3-c6b7-4cef-b824-ae1a7304b3e5	caf4ee17-2606-45a5-8a62-e8536677c69c	1	Salle 1 : un Orc vous attend...	0	{0,2,1}	\N	\N	\N	\N	\N	0d59e757-ef78-4164-83a8-b1fdfbafc9bb	f	2	orc.png	Orc	10	\N	\N
44740e9b-0ea7-4556-9204-e22fcad502fb	2f83fc20-4b0c-4519-a057-bc0e33ba0882	2	Salle 2 : un Dragonnet vous attend...	0	{0,2,1}	\N	\N	\N	\N	\N	abcfe7c2-9a0f-4883-8c54-2f951cc6d659	f	4	dragon.png	Dragonnet	20	\N	\N
63e1f417-8734-48ea-817a-a5de75589598	2f83fc20-4b0c-4519-a057-bc0e33ba0882	1	Salle 1 : un Dragonnet vous attend...	0	{0,2,1}	\N	\N	\N	\N	\N	abcfe7c2-9a0f-4883-8c54-2f951cc6d659	f	2	dragon.png	Dragonnet	10	\N	\N
b22d51c7-f307-4ebd-8d89-4ce55761a477	2f83fc20-4b0c-4519-a057-bc0e33ba0882	3	Salle 3 : un Orc vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	abcfe7c2-9a0f-4883-8c54-2f951cc6d659	f	6	orc.png	Orc	30	\N	\N
fd98970d-8ec1-49cf-bfbf-fc1bf59e9959	2f83fc20-4b0c-4519-a057-bc0e33ba0882	4	Salle 4 : un Dragonnet vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	abcfe7c2-9a0f-4883-8c54-2f951cc6d659	f	8	dragon.png	Dragonnet	40	\N	\N
3dedff1a-21ec-49ee-8a48-0c8ff63465c3	fa0f2fb4-877b-4ce2-992b-fcbf922defe4	2	Salle 2 : un Orc vous attend...	0	{0,2,1}	\N	\N	\N	\N	\N	a830db74-03a7-49c9-9c32-55af73de25fe	f	4	orc.png	Orc	20	\N	\N
5b0249f4-91a4-4712-99d8-9a88a66fd722	fa0f2fb4-877b-4ce2-992b-fcbf922defe4	3	Salle 3 : un Squelette vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	a830db74-03a7-49c9-9c32-55af73de25fe	f	6	skeleton.png	Squelette	30	\N	\N
5ec6ffb4-975b-4063-a54b-4de13cdb7e16	fa0f2fb4-877b-4ce2-992b-fcbf922defe4	1	Salle 1 : un Squelette vous attend...	0	{0,2,1}	\N	\N	\N	\N	\N	a830db74-03a7-49c9-9c32-55af73de25fe	f	2	skeleton.png	Squelette	10	\N	\N
8e01aea7-1c04-4dc6-af71-e95e1f7b0012	fa0f2fb4-877b-4ce2-992b-fcbf922defe4	4	Salle 4 : un Squelette vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	a830db74-03a7-49c9-9c32-55af73de25fe	f	8	skeleton.png	Squelette	40	\N	\N
1b7a7a74-cf1c-46fc-b025-f3f96a74916a	aa0eef7c-a432-41c8-9769-ac5896b99ef9	3	Salle 3 : un Dragonnet vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	b1817c80-036a-47f7-a428-89123819edcd	f	6	dragon.png	Dragonnet	30	\N	\N
e5ac01c7-81c1-4e45-932c-8f3a44d3c1f5	aa0eef7c-a432-41c8-9769-ac5896b99ef9	2	Salle 2 : un Dragonnet vous attend...	0	{0,2,1}	\N	\N	\N	\N	\N	b1817c80-036a-47f7-a428-89123819edcd	f	4	dragon.png	Dragonnet	20	\N	\N
e94d9b67-54f7-4ac3-a266-10c12ebbbd70	aa0eef7c-a432-41c8-9769-ac5896b99ef9	4	Salle 4 : un Gobelin vous attend...	1	{0,2,1}	\N	\N	\N	\N	\N	b1817c80-036a-47f7-a428-89123819edcd	f	8	goblin.png	Gobelin	40	\N	\N
ffef67fe-0b8b-465e-8d40-4895acd8ec47	aa0eef7c-a432-41c8-9769-ac5896b99ef9	1	Salle 1 : un Orc vous attend...	0	{0,2,1}	\N	\N	\N	\N	\N	b1817c80-036a-47f7-a428-89123819edcd	f	2	orc.png	Orc	10	\N	\N
4f93d2dd-3e8a-4c5f-9b14-ea1539e07877	17f8be0b-1a2b-474d-94fc-c991ade5a8a8	1	Salle 1 : un Gobelin vous attend...	0	{0,2,1}	0	0	0	f	Échec... Le Gobelin vous a blessé.	d6d1df3a-6a6d-4f7a-a72c-adf94d17f4e7	t	2	goblin.png	Gobelin	10	30	0
8b1c4177-78a5-4de5-bade-947b54c9ba0f	17f8be0b-1a2b-474d-94fc-c991ade5a8a8	2	Salle 2 : un Dragonnet vous attend...	0	{0,2,1}	0	0	0	f	Échec... Le Dragonnet vous a blessé.	d6d1df3a-6a6d-4f7a-a72c-adf94d17f4e7	t	4	dragon.png	Dragonnet	20	30	0
be14c74a-9380-4bbf-934b-753d37825132	17f8be0b-1a2b-474d-94fc-c991ade5a8a8	3	Salle 3 : un Squelette vous attend...	1	{0,2,1}	0	0	50	f	Victoire ! Vous avez terrassé le Squelette. (FIN DU DONJON)	d6d1df3a-6a6d-4f7a-a72c-adf94d17f4e7	t	6	skeleton.png	Squelette	0	30	50
d4ad88da-e4b6-4e44-8c56-213b6d51ac93	c119c61b-11a5-4fb2-9594-e06bb998afc1	1	Salle 1 : un Gobelin vous attend...	0	{0,2,1}	0	0	0	f	Victoire ! Vous avez terrassé le Gobelin.	55a4259b-6924-4512-8eb5-6c7d7fbdf1b0	t	2	goblin.png	Gobelin	0	30	0
c28decbf-b413-4465-95ff-1d1412ca6bcf	c119c61b-11a5-4fb2-9594-e06bb998afc1	2	Salle 2 : un Orc vous attend...	0	{0,2,1}	0	0	0	f	Échec... Le Orc vous a blessé.	55a4259b-6924-4512-8eb5-6c7d7fbdf1b0	t	4	orc.png	Orc	20	30	0
277e1b50-4fec-43b8-b2fb-d4d0d88aaee1	4784d565-adc3-4246-90e0-eba3c2aceaa0	2	Salle 2 : un Dragonnet vous attend...	0	{0,2,1}	2	2	-10	t	C'était un piège ! Vous perdez des PV.	9c2a7fa3-036a-4e0f-95c9-ef12b2355e48	t	4	dragon.png	Dragonnet	20	50	-10
6eaa947a-fa27-4466-9157-5c03f6daf9b9	4784d565-adc3-4246-90e0-eba3c2aceaa0	3	Salle 3 : un Orc vous attend...	1	{0,2,1}	0	0	50	f	Victoire ! Vous avez terrassé le Orc.	9c2a7fa3-036a-4e0f-95c9-ef12b2355e48	t	6	orc.png	Orc	0	30	40
93b037d9-7470-4307-867e-dd037dd7a251	4784d565-adc3-4246-90e0-eba3c2aceaa0	4	Salle 4 : un Orc vous attend...	1	{0,2,1}	1	1	10	f	Vous avez fui prudemment et restez en vie.	9c2a7fa3-036a-4e0f-95c9-ef12b2355e48	t	8	orc.png	Orc	40	0	50
e2f4cb15-6b55-4f74-8dc7-fa8899a65569	4784d565-adc3-4246-90e0-eba3c2aceaa0	5	Salle 5 : un Squelette vous attend...	2	{0,2,1}	2	2	-10	t	C'était un piège ! Vous perdez des PV. (FIN DU DONJON)	9c2a7fa3-036a-4e0f-95c9-ef12b2355e48	t	10	skeleton.png	Squelette	50	50	40
28424d26-6389-42dc-957f-ec983916d312	c119c61b-11a5-4fb2-9594-e06bb998afc1	3	Salle 3 : un Orc vous attend...	1	{0,2,1}	0	0	0	f	Échec... Le Orc vous a blessé.	55a4259b-6924-4512-8eb5-6c7d7fbdf1b0	t	6	orc.png	Orc	30	30	0
9afc7009-0eec-4bae-aa4d-438b67346f25	c119c61b-11a5-4fb2-9594-e06bb998afc1	4	Salle 4 : un Squelette vous attend...	1	{0,2,1}	1	1	10	f	Vous avez fui prudemment et restez en vie. (FIN DU DONJON)	55a4259b-6924-4512-8eb5-6c7d7fbdf1b0	t	8	skeleton.png	Squelette	40	0	10
fdc5242d-63ab-44e8-9a43-fe11f4898461	4784d565-adc3-4246-90e0-eba3c2aceaa0	1	Salle 1 : un Orc vous attend...	0	{0,2,1}	0	0	0	f	Victoire ! Vous avez terrassé le Orc.	9c2a7fa3-036a-4e0f-95c9-ef12b2355e48	t	2	orc.png	Orc	0	30	0
\.


--
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20251025123004_InitialCreate	9.0.10
20251025150156_AddAdministrateurAndDonjon	9.0.10
20251121151047_AddEstVisiteeToSalle	9.0.10
20251123172439_V3_ActionResultat_Extended	9.0.10
\.


--
-- Name: Administrateurs PK_Administrateurs; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Administrateurs"
    ADD CONSTRAINT "PK_Administrateurs" PRIMARY KEY ("Id");


--
-- Name: Donjons PK_Donjons; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Donjons"
    ADD CONSTRAINT "PK_Donjons" PRIMARY KEY ("Id");


--
-- Name: Joueurs PK_Joueurs; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Joueurs"
    ADD CONSTRAINT "PK_Joueurs" PRIMARY KEY ("Id");


--
-- Name: Parties PK_Parties; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Parties"
    ADD CONSTRAINT "PK_Parties" PRIMARY KEY ("Id");


--
-- Name: Salles PK_Salles; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Salles"
    ADD CONSTRAINT "PK_Salles" PRIMARY KEY ("Id");


--
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- Name: IX_Joueurs_AdministrateurId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Joueurs_AdministrateurId" ON public."Joueurs" USING btree ("AdministrateurId");


--
-- Name: IX_Parties_DonjonId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Parties_DonjonId" ON public."Parties" USING btree ("DonjonId");


--
-- Name: IX_Parties_JoueurId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Parties_JoueurId" ON public."Parties" USING btree ("JoueurId");


--
-- Name: IX_Salles_DonjonId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Salles_DonjonId" ON public."Salles" USING btree ("DonjonId");


--
-- Name: IX_Salles_PartieId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Salles_PartieId" ON public."Salles" USING btree ("PartieId");


--
-- Name: Joueurs FK_Joueurs_Administrateurs_AdministrateurId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Joueurs"
    ADD CONSTRAINT "FK_Joueurs_Administrateurs_AdministrateurId" FOREIGN KEY ("AdministrateurId") REFERENCES public."Administrateurs"("Id");


--
-- Name: Parties FK_Parties_Donjons_DonjonId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Parties"
    ADD CONSTRAINT "FK_Parties_Donjons_DonjonId" FOREIGN KEY ("DonjonId") REFERENCES public."Donjons"("Id") ON DELETE CASCADE;


--
-- Name: Parties FK_Parties_Joueurs_JoueurId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Parties"
    ADD CONSTRAINT "FK_Parties_Joueurs_JoueurId" FOREIGN KEY ("JoueurId") REFERENCES public."Joueurs"("Id") ON DELETE CASCADE;


--
-- Name: Salles FK_Salles_Donjons_DonjonId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Salles"
    ADD CONSTRAINT "FK_Salles_Donjons_DonjonId" FOREIGN KEY ("DonjonId") REFERENCES public."Donjons"("Id");


--
-- Name: Salles FK_Salles_Parties_PartieId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Salles"
    ADD CONSTRAINT "FK_Salles_Parties_PartieId" FOREIGN KEY ("PartieId") REFERENCES public."Parties"("Id") ON DELETE CASCADE;


--
-- PostgreSQL database dump complete
--

\unrestrict RkAG0x7n8hTdRNFNbhwMzuvVuUGWcXAVi6Lycq7oeqkQLhKuG7pAnMX6ZRQ3QxD

