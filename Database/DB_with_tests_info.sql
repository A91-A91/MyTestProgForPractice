--
-- PostgreSQL database dump
--

\restrict oO6zcSzhOBV5jazPG1awOEwCK4yhWF852ubUlkjrSPlM3bdnc7NotneUfjJIPHO

-- Dumped from database version 18.4
-- Dumped by pg_dump version 18.4

-- Started on 2026-07-31 15:22:27

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
-- TOC entry 219 (class 1259 OID 17418)
-- Name: Results; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Results" (
    id integer NOT NULL,
    "fileName" text,
    "timeDelta" double precision,
    "startDate" timestamp with time zone,
    average_exec_time double precision,
    average_value double precision,
    median_value double precision,
    max_value double precision,
    min_value double precision
);


ALTER TABLE public."Results" OWNER TO postgres;

--
-- TOC entry 222 (class 1259 OID 17454)
-- Name: Results_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."Results" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Results_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 220 (class 1259 OID 17426)
-- Name: Values; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Values" (
    id integer NOT NULL,
    date timestamp with time zone,
    execution_time double precision,
    "valueData" double precision,
    result_id integer
);


ALTER TABLE public."Values" OWNER TO postgres;

--
-- TOC entry 221 (class 1259 OID 17447)
-- Name: Values_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."Values" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Values_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 5013 (class 0 OID 17418)
-- Dependencies: 219
-- Data for Name: Results; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Results" (id, "fileName", "timeDelta", "startDate", average_exec_time, average_value, median_value, max_value, min_value) FROM stdin;
4	correct1.csv	1140	2026-07-26 13:00:00+03	1.14	20.58	20.85	29.9	10.5
3	oneRow.csv	0	2026-07-20 13:00:00+03	2.15	50	50	50	50
5	duplicateFile.csv	15	2026-07-30 13:00:00+03	1.22	15.419999999999998	15.8	20.1	10.5
6	correct.csv	15	2026-07-26 13:00:00+03	1.22	15.419999999999998	15.8	20.1	10.5
\.


--
-- TOC entry 5014 (class 0 OID 17426)
-- Dependencies: 220
-- Data for Name: Values; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Values" (id, date, execution_time, "valueData", result_id) FROM stdin;
16	2026-07-26 13:00:00+03	2.15	50	3
57	2026-07-26 13:00:00+03	1.1	10.5	4
58	2026-07-26 13:01:00+03	1.2	11.2	4
59	2026-07-26 13:02:00+03	0.9	12.7	4
60	2026-07-26 13:03:00+03	1.3	13.4	4
61	2026-07-26 13:04:00+03	1	14.1	4
62	2026-07-26 13:05:00+03	1.4	15.8	4
63	2026-07-26 13:06:00+03	1.1	16.3	4
64	2026-07-26 13:07:00+03	0.8	17.9	4
65	2026-07-26 13:08:00+03	1.5	18.2	4
66	2026-07-26 13:09:00+03	1.2	19.5	4
67	2026-07-26 13:10:00+03	0.7	20.1	4
68	2026-07-26 13:11:00+03	1.3	21.6	4
69	2026-07-26 13:12:00+03	1	22.4	4
70	2026-07-26 13:13:00+03	1.6	23.7	4
71	2026-07-26 13:14:00+03	1.1	24.8	4
72	2026-07-26 13:15:00+03	0.9	25.3	4
73	2026-07-26 13:16:00+03	1.2	26.9	4
74	2026-07-26 13:17:00+03	1.4	27.5	4
75	2026-07-26 13:18:00+03	1	28.6	4
76	2026-07-26 13:19:00+03	1.3	29.9	4
77	2026-07-26 13:00:00+03	1.25	10.5	5
78	2026-07-26 13:00:05+03	1.1	15.8	5
79	2026-07-26 13:00:08+03	0.95	12.3	5
80	2026-07-26 13:00:10+03	1.5	20.1	5
81	2026-07-26 13:00:15+03	1.3	18.4	5
82	2026-07-26 13:00:00+03	1.25	10.5	6
83	2026-07-26 13:00:05+03	1.1	15.8	6
84	2026-07-26 13:00:08+03	0.95	12.3	6
85	2026-07-26 13:00:10+03	1.5	20.1	6
86	2026-07-26 13:00:15+03	1.3	18.4	6
\.


--
-- TOC entry 5022 (class 0 OID 0)
-- Dependencies: 222
-- Name: Results_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Results_id_seq"', 6, true);


--
-- TOC entry 5023 (class 0 OID 0)
-- Dependencies: 221
-- Name: Values_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Values_id_seq"', 86, true);


--
-- TOC entry 4862 (class 2606 OID 17425)
-- Name: Results Results_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Results"
    ADD CONSTRAINT "Results_pkey" PRIMARY KEY (id);


--
-- TOC entry 4864 (class 2606 OID 17431)
-- Name: Values Values_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Values"
    ADD CONSTRAINT "Values_pkey" PRIMARY KEY (id);


--
-- TOC entry 4865 (class 2606 OID 17432)
-- Name: Values fk_result_id ; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Values"
    ADD CONSTRAINT "fk_result_id " FOREIGN KEY (result_id) REFERENCES public."Results"(id);


-- Completed on 2026-07-31 15:22:28

--
-- PostgreSQL database dump complete
--

\unrestrict oO6zcSzhOBV5jazPG1awOEwCK4yhWF852ubUlkjrSPlM3bdnc7NotneUfjJIPHO

