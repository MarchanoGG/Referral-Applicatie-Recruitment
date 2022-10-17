--
-- PostgreSQL database dump
--

-- Dumped from database version 14.5
-- Dumped by pg_dump version 14.5

-- Started on 2022-10-13 20:40:10

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
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
-- TOC entry 213 (class 1259 OID 16432)
-- Name: candidates; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.candidates (
    object_key integer NOT NULL,
    fk_profile integer NOT NULL,
    reffered_at timestamp without time zone NOT NULL
);


ALTER TABLE public.candidates OWNER TO postgres;

--
-- TOC entry 210 (class 1259 OID 16402)
-- Name: profiles; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.profiles (
    object_key integer NOT NULL,
    initials character varying(10),
    name character varying(40),
    surname character varying(70),
    email character varying(40),
    phone_number character varying(30),
    address character varying(10)
);


ALTER TABLE public.profiles OWNER TO postgres;

--
-- TOC entry 215 (class 1259 OID 16447)
-- Name: referrals; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.referrals (
    object_key integer NOT NULL,
    fk_user integer NOT NULL,
    fk_task integer NOT NULL,
    fk_candidate integer NOT NULL,
    fk_scoreboard integer NOT NULL,
    creation_dt timestamp without time zone NOT NULL,
    modification_dt timestamp without time zone NOT NULL
);


ALTER TABLE public.referrals OWNER TO postgres;

--
-- TOC entry 211 (class 1259 OID 16412)
-- Name: rewards; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.rewards (
    object_key integer NOT NULL,
    fk_user integer NOT NULL,
    name character varying(50) NOT NULL,
    award_dt timestamp without time zone NOT NULL
);


ALTER TABLE public.rewards OWNER TO postgres;

--
-- TOC entry 212 (class 1259 OID 16422)
-- Name: scoreboards; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.scoreboards (
    object_key integer NOT NULL,
    fk_user integer NOT NULL,
    name character varying(40) NOT NULL,
    start_dt timestamp without time zone NOT NULL,
    end_dt timestamp without time zone
);


ALTER TABLE public.scoreboards OWNER TO postgres;

--
-- TOC entry 214 (class 1259 OID 16442)
-- Name: tasks; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tasks (
    object_key integer NOT NULL,
    name character varying(40) NOT NULL,
    points integer NOT NULL,
    description character varying(255)
);


ALTER TABLE public.tasks OWNER TO postgres;

--
-- TOC entry 209 (class 1259 OID 16395)
-- Name: users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users (
    object_key integer NOT NULL,
    fk_profile integer,
    username character varying(100) NOT NULL,
    password character varying(255) NOT NULL,
    recruiter bit(1) NOT NULL,
    creation_dt timestamp without time zone NOT NULL,
    modification_dt timestamp without time zone NOT NULL
);


ALTER TABLE public.users OWNER TO postgres;

--
-- TOC entry 3354 (class 0 OID 16432)
-- Dependencies: 213
-- Data for Name: candidates; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3351 (class 0 OID 16402)
-- Dependencies: 210
-- Data for Name: profiles; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3356 (class 0 OID 16447)
-- Dependencies: 215
-- Data for Name: referrals; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3352 (class 0 OID 16412)
-- Dependencies: 211
-- Data for Name: rewards; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3353 (class 0 OID 16422)
-- Dependencies: 212
-- Data for Name: scoreboards; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3355 (class 0 OID 16442)
-- Dependencies: 214
-- Data for Name: tasks; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3350 (class 0 OID 16395)
-- Dependencies: 209
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3198 (class 2606 OID 16436)
-- Name: candidates candidates_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.candidates
    ADD CONSTRAINT candidates_pkey PRIMARY KEY (object_key);


--
-- TOC entry 3192 (class 2606 OID 16406)
-- Name: profiles profiles_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.profiles
    ADD CONSTRAINT profiles_pkey PRIMARY KEY (object_key);


--
-- TOC entry 3202 (class 2606 OID 16451)
-- Name: referrals referrals_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.referrals
    ADD CONSTRAINT referrals_pkey PRIMARY KEY (object_key);


--
-- TOC entry 3194 (class 2606 OID 16416)
-- Name: rewards rewards_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.rewards
    ADD CONSTRAINT rewards_pkey PRIMARY KEY (object_key);


--
-- TOC entry 3196 (class 2606 OID 16426)
-- Name: scoreboards scoreboards_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.scoreboards
    ADD CONSTRAINT scoreboards_pkey PRIMARY KEY (object_key);


--
-- TOC entry 3200 (class 2606 OID 16446)
-- Name: tasks tasks_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tasks
    ADD CONSTRAINT tasks_pkey PRIMARY KEY (object_key);


--
-- TOC entry 3188 (class 2606 OID 16399)
-- Name: users users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (object_key);


--
-- TOC entry 3190 (class 2606 OID 16401)
-- Name: users users_username_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_username_key UNIQUE (username);


--
-- TOC entry 3206 (class 2606 OID 16437)
-- Name: candidates candidates_fk_profile_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.candidates
    ADD CONSTRAINT candidates_fk_profile_fkey FOREIGN KEY (fk_profile) REFERENCES public.users(object_key);


--
-- TOC entry 3203 (class 2606 OID 16407)
-- Name: users fk_users_profile; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT fk_users_profile FOREIGN KEY (fk_profile) REFERENCES public.profiles(object_key);


--
-- TOC entry 3209 (class 2606 OID 16462)
-- Name: referrals referrals_fk_candidate_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.referrals
    ADD CONSTRAINT referrals_fk_candidate_fkey FOREIGN KEY (fk_candidate) REFERENCES public.candidates(object_key);


--
-- TOC entry 3210 (class 2606 OID 16467)
-- Name: referrals referrals_fk_scoreboard_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.referrals
    ADD CONSTRAINT referrals_fk_scoreboard_fkey FOREIGN KEY (fk_scoreboard) REFERENCES public.scoreboards(object_key);


--
-- TOC entry 3208 (class 2606 OID 16457)
-- Name: referrals referrals_fk_task_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.referrals
    ADD CONSTRAINT referrals_fk_task_fkey FOREIGN KEY (fk_task) REFERENCES public.tasks(object_key);


--
-- TOC entry 3207 (class 2606 OID 16452)
-- Name: referrals referrals_fk_user_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.referrals
    ADD CONSTRAINT referrals_fk_user_fkey FOREIGN KEY (fk_user) REFERENCES public.users(object_key);


--
-- TOC entry 3204 (class 2606 OID 16417)
-- Name: rewards rewards_fk_user_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.rewards
    ADD CONSTRAINT rewards_fk_user_fkey FOREIGN KEY (fk_user) REFERENCES public.users(object_key);


--
-- TOC entry 3205 (class 2606 OID 16427)
-- Name: scoreboards scoreboards_fk_user_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.scoreboards
    ADD CONSTRAINT scoreboards_fk_user_fkey FOREIGN KEY (fk_user) REFERENCES public.users(object_key);


-- Completed on 2022-10-13 20:40:11

--
-- PostgreSQL database dump complete
--

